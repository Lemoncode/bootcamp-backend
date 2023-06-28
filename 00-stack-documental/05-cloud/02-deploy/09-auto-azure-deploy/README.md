# 09 Automatic Azure deploy

In this example we are going to deploy app to Azure using Docker

We will start from `08-auto-render-deploy`.

# Steps to build it

`npm install` to install previous sample packages:

```bash
cd front
npm install

```

In a second terminal:

```bash
cd back
npm install

```

Create new repository and upload files:

![01-create-repo](./readme-resources/01-create-repo.png)

```bash
git init
git remote add origin git@github.com...
git add .
git commit -m "initial commit"
git push -u origin main

```

## Create Azure Service Plan

Create a new Azure Service Plan:

![02-create-azure-service-plan](./readme-resources/02-create-azure-service-plan.png)

> We have around 10 apps in the free tier for each Azure Service Plan.
>
> 1 free tier by region.

Configure the new service plan instance details:

![03-configure-service-plan](./readme-resources/03-configure-service-plan.png)

Configure pricing plans:

![04-configure-pricing](./readme-resources/04-configure-pricing.png)

![05-review-and-create](./readme-resources/05-review-and-create.png)

## Create Azure App Service

Create a new Azure app:

![06-create-azure-app](./readme-resources/06-create-azure-app.png)

Configure the new app instance details:

![07-configure-web-app](./readme-resources/07-configure-web-app.png)

Configure pricing plans:

![08-configure-pricing](./readme-resources/08-configure-pricing.png)

![09-review-and-create](./readme-resources/09-review-and-create.png)

As we can see, this app will deploy an example of Microsoft Docker Image. We can provide our custom Docker image in the configuration section using environment variables:

![10-configuration-section](./readme-resources/10-configuration-section.png)

> [Offical Docs deploy to azure app](https://docs.github.com/en/actions/deployment/deploying-to-your-cloud-provider/deploying-to-azure/deploying-docker-to-azure-app-service)

If we want a public image we can use the previous uploaded image that we push to Docker Hub in the previous example or we can upload a private one using [Github Packages](https://github.com/features/packages).

> [Official Github Container Registry Docs](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-container-registry)

Create a new Github Personal Access Token:

![11-perfil-settings](./readme-resources/11-perfil-settings.png)

![12-developer-settings](./readme-resources/12-developer-settings.png)

![13-personal-access-token](./readme-resources/13-personal-access-token.png)

![14-name-and-expiration](./readme-resources/14-name-and-expiration.png)

![15-token-scopes](./readme-resources/15-token-scopes.png)

Clicks on _Generate token_ button and update values in Azure configuration section:

![16-update-azure-values](./readme-resources/16-update-azure-values.png)

> DOCKER_REGISTRY_SERVER_URL: `https://ghcr.io`
>
> DOCKER_REGISTRY_SERVER_USERNAME: use your Github username or organization name instead of `lemoncode`.

Add environment variables to our app:

![17-add-env-vars](./readme-resources/17-add-env-vars.png)

Now we can create a Github Action workflow to deploy our app to Azure:

_./.github/workflows/cd.yml_

```yml
name: CD Workflow

on:
  push:
    branches:
      - main

env:
  IMAGE_NAME: ghcr.io/${{github.repository}}:${{github.run_number}}-${{github.run_attempt}}

permissions:
  contents: "read"
  packages: "write"
```

> `github.repository`: The repository name with the owner. For example, `octocat/hello-world`. You only can use this variable if it's lower case due to a Docker tag restriction: `--tag" flag: invalid reference format: repository name must be lowercase`
> 
> For example, you can use `github.run_number` and `github.run_attempt` to create a unique image tag. But you can use any other tag.
>
> [Github context API](https://docs.github.com/en/actions/learn-github-actions/contexts#github-context)

Define job to build and push image to Github Container Registry:

_./.github/workflows/cd.yml_

```diff
...
permissions:
  contents: 'read'
  packages: 'write'

+ jobs:
+   cd:
+     runs-on: ubuntu-latest
+     steps:
+       - name: Checkout repository
+         uses: actions/checkout@v3

+       - name: Log in to GitHub container registry
+         uses: docker/login-action@v2
+         with:
+           registry: ghcr.io
+           username: ${{ github.actor }}
+           password: ${{ secrets.GITHUB_TOKEN }}

+       - name: Build and push docker image
+         run: |
+           docker build -t ${{env.IMAGE_NAME}} .
+           docker push ${{env.IMAGE_NAME}}

+       - name: Deploy to Azure
+         uses: azure/webapps-deploy@v2
+         with:
+           app-name: ${{ secrets.AZURE_APP_NAME }}
+           publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE }}
+           images: ${{env.IMAGE_NAME}}

```

> [GITHUB_TOKEN automatic token authentication](https://docs.github.com/en/actions/security-guides/automatic-token-authentication)

Create `secrets` in Github repository:

![18-create-repository-secrets](./readme-resources/18-create-repository-secrets.png)

- `AZURE_APP_NAME` (same value as you name your app in Azure):

![19-app-name](./readme-resources/19-app-name.png)

- `AZURE_PUBLISH_PROFILE` (you can download it from Azure portal and paste the value in the secret):

![20-download-publish-profile](./readme-resources/20-download-publish-profile.png)

![21-publish-profile](./readme-resources/21-publish-profile.png)

Upload changes:

```bash
git add .
git commit -m "create github workflow"
git push

```

After the successful deploy, open `https://<app-name>.azurewebsites.net`.

# ¿Con ganas de aprender Backend?

En Lemoncode impartimos un Bootcamp Backend Online, centrado en stack node y stack .net, en él encontrarás todos los recursos necesarios: clases de los mejores profesionales del sector, tutorías en cuanto las necesites y ejercicios para desarrollar lo aprendido en los distintos módulos. Si quieres saber más puedes pinchar [aquí para más información sobre este Bootcamp Backend](https://lemoncode.net/bootcamp-backend#bootcamp-backend/banner).

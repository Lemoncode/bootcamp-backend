# Stripe Notifications

In this example we are going to configure a webhook to receive notifications from Stripe in our backend when a payment has been completed.

## Steps

We will start from _01-stripe-checkout_. Let's install the dependencies and run the project.

```bash
npm install
npm start
```

Let's define the webhook endpoint in our API file.

_./src/api.ts_:

```ts
api.post('/webhook', (req, res) => {
  const payload = req.body;

  // Just show in the console what Stripe sent
  console.log(`Got payload: ${JSON.stringify(payload)}`);

  res.status(200).end();
});
```

If we execute this and we complete a successful payment, we can notice that endpoint has not been called. What's going on? Well, Stripe is not able no find localhost, which is normal.

We could upload the webhook endpoint to a cloud provider and define/configure it from [Stripe dashboard](https://dashboard.stripe.com/test/webhooks/create) but, it would take some time.

Is there no other option? Yes, making use of `stripe-cli`.

First of all, we need to install it in our machine. You can follow [this guide](https://stripe.com/docs/stripe-cli) from Stripe docs. Depending on your OS, the steps may change.

- macOC

```bash
brew install stripe/stripe-cli/stripe
```

- Ubuntu/Debian

```bash
# Add Stripe CLI’s GPG key to the apt sources keyring
curl -s https://packages.stripe.dev/api/security/keypair/stripe-cli-gpg/public | gpg --dearmor | sudo tee /usr/share/keyrings/stripe.gpg
# Add CLI’s apt repository to the apt sources list
echo "deb [signed-by=/usr/share/keyrings/stripe.gpg] https://packages.stripe.dev/stripe-cli-debian-local stable main" | sudo tee -a /etc/apt/sources.list.d/stripe.list
# Update the package list
sudo apt update
# Install the CLI
sudo apt install stripe
```

- Windows

Download it from [this link](https://github.com/stripe/stripe-cli/releases/latest), choose from the list _stripe_1.X.X_windows_x86_64.zip_, unzip it and execute exe file.

In order to have access to the CLI anywhere within your machine you need to add the route where the exe is located in `PATH` env. variable.

A Stripe account is a requirement here too. You need your development keys.

Copy your private key in our environment variables file

_.env.example_:

```diff
NODE_ENV=development
PORT=8081
STATIC_FILES_PATH=../public
- STRIPE_SECRET=sk_test_7mJuPfZsBzc3JkrANrFrcDqC
+ STRIPE_SECRET=**PASTE YOUR STRIPE SECRET KEY HERE (starts by sk_text)**
```

Log in using `stripe login` and authorize your browser.

```bash
stripe login
```

Let's tell Stripe Stripe to redirect all development notifications to our webhook `localhost:8081/api/webhook`

```bash
stripe listen --forward-to localhost:8081/api/webhook
```

Now we can see in the console the received payload after coming back from Stripe payment gateway. We can add few more `console.log` to see more clearly some fields form the received object: id, type, create date or API version.

_./src/api.ts_:

```diff
api.post('/webhook', (req, res) => {
  const payload = req.body;

  // Just show in the console what Stripe sent
  console.log(`Got payload: ${JSON.stringify(payload)}`);

+ console.log(`Id: ${req.body.id}`);
+ console.log(`Type: ${req.body.type}`);
+ console.log(`Creation date: ${new Date(req.body.created)}`);
+ console.log(`API version: ${req.body.api_version}`);

  res.status(200).end();
});
```

Command `stripe listen` gives us a secret to validate that the received notifications come from Stripe. We copy it from the console (it starts by _whsec__).

![stripe listen](./resources/stripe_listen.png)

Let's go back to API file. We need to verify signature in `webhook` endpoint.

_./src/api.ts_:

```diff
+ // Paste here signing secret given by _stripe listen --forward_ (it starts by por whsec_)
+ const signingSecret = "whsec_...";

api.post('/webhook', (req, res) => {
  const payload = request.body;
+ const sig = req.headers['stripe-signature'];

  // Just show in the console what Stripe sent
  console.log('Got payload: ' + JSON.stringify(payload));

  console.log(`Id: ${req.body.id}`);
  console.log(`Type: ${req.body.type}`);
  console.log(`Creation date: ${new Date(req.body.created)}`);
  console.log(`API version: ${req.body.api_version}`);

+  try {
+    // We use Stripe library to validate the response using the signingSecret.
+    // This allows us to verify if the notification comes or not from Stripe.
+    stripe.webhooks.constructEvent(payload, sig, signingSecret);
+  } catch (err) {
+    return res.status(400).send(`Webhook Error: ${err.message}`);
+  }

  res.status(200).end();
});
```

Let's try this out again and check what is going on (do not close terminal in which `stripe listen` is executed).

```bash
npm start
```

But validation is not correct. Why? Body is parsed and we need the raw, original received data.

_./src/express.server.ts_:

```diff
  app.use(
-    express.json({
+    express.json({
+      verify: function (req, res, buf) {
+        req['raw'] = buf;
+      },
+    })
-    )
  );
```

Let's update webhook too.

_./src/api.ts_:

```diff
api.post('/webhook', (req, res) => {
  const payload = req.body;
  const sig = req.headers['stripe-signature'];
+ const rawBody = req['raw'];

  // Just show in the console what Stripe sent
  console.log(`Got payload: ${JSON.stringify(payload)}`);

  console.log(`Id: ${req.body.id}`);
  console.log(`Type: ${req.body.type}`);
  console.log(`Creation date: ${new Date(req.body.created)}`);
  console.log(`API version: ${req.body.api_version}`);

  try {
    // We use Stripe library to validate the response using the signingSecret.
    // This allows us to verify if the notification comes or not from Stripe.
-   stripe.webhooks.constructEvent(payload, sig, signingSecret);
+   stripe.webhooks.constructEvent(rawBody, sig, signingSecret);
  } catch (err) {
    return res.status(400).send(`Webhook Error: ${err.message}`);
  }

  response.status(200).end();
});
```

If we try again, we can check that the validation is correct.

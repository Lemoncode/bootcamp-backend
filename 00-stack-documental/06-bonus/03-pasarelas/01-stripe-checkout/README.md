# Stripe Checkout

In this example we are going to configure stripe checkout to make a payment. Here, we are not going to notify server, we will do it in the next example.

## Steps

We will start from _00-boilerplate_. Let's install the dependencies and run the project.

```bash
npm install
npm start
```

- In http://localhost:8081 we have a home page
- In http://localhost:8081/api there is and endpoint which returns data.

Now, we will stop app execution and install stripe library. In this case, library has typings included.

```bash
npm install stripe
```

Let's create a stylesheet to give our pages a better look.

_./public/styles.css_:

```css
body {
  display: flex;
  justify-content: center;
  align-items: center;
  background: #242d60;
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto',
    'Helvetica Neue', 'Ubuntu', sans-serif;
  height: 100vh;
  margin: 0;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
section {
  background: #ffffff;
  display: flex;
  flex-direction: column;
  width: 400px;
  height: 112px;
  border-radius: 6px;
  justify-content: space-between;
}
.product {
  display: flex;
}
.description {
  display: flex;
  flex-direction: column;
  justify-content: center;
}
p {
  font-style: normal;
  font-weight: 500;
  font-size: 14px;
  line-height: 20px;
  letter-spacing: -0.154px;
  color: #242d60;
  height: 100%;
  width: 100%;
  padding: 0 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  box-sizing: border-box;
}
img {
  border-radius: 6px;
  margin: 10px;
  width: 54px;
  height: 57px;
}
h3,
h5 {
  font-style: normal;
  font-weight: 500;
  font-size: 14px;
  line-height: 20px;
  letter-spacing: -0.154px;
  color: #242d60;
  margin: 0;
}
h5 {
  opacity: 0.5;
}
#checkout-button {
  height: 36px;
  background: #556cd6;
  color: white;
  width: 100%;
  font-size: 14px;
  border: 0;
  font-weight: 500;
  cursor: pointer;
  letter-spacing: 0.6;
  border-radius: 0 0 6px 6px;
  transition: all 0.2s ease;
  box-shadow: 0px 4px 5.5px 0px rgba(0, 0, 0, 0.07);
}
#checkout-button:hover {
  opacity: 0.8;
}
```

And now we will update the html file (replace with the content below).

_./public/index.html_:

```html
<!DOCTYPE html>
<html>
  <head>
    <title>Buy cool new product</title>
    <link rel="stylesheet" href="styles.css" />
    <script src="https://polyfill.io/v3/polyfill.min.js?version=3.52.1&features=fetch"></script>
  </head>
  <body>
    <section>
      <div class="product">
        <img
          src="https://i.imgur.com/EHyR2nP.png"
          alt="The cover of Stubborn Attachments"
        />
        <div class="description">
          <h3>Stubborn Attachments</h3>
          <h5>$20.00</h5>
        </div>
      </div>
      <button id="checkout-button">Checkout</button>
    </section>
  </body>
</html>
```

Let's check what we have. Restart app (`npm start`), open your favourite browser and navigate to http://localhost:8081. As you can see, we already have the checkout page, but without functionality.

Next step is to create a stripe session for this payment. The flow is as follows:

- Create session via back channel (from our server to stripe server, this way we can sign the request with a shared secret/key).
- Once we have the session id in our server, we pass it to the front. This way UI can redirect the app flow to stripe checkout page.

To create a session we need an account in stripe. For development purposes, stripe gives us a couple of keys, a public key and a private key. The latter will be stored as an environment variable. **Remember to not push that private key to your repository**.

In this example we are going to use a generic key (check [this link](https://stripe.com/docs/checkout/quickstart?lang=node#init-stripe) to get the generic key):

_.env.example_:

```diff
NODE_ENV=development
PORT=8081
STATIC_FILES_PATH=../public
+STRIPE_SECRET=sk_test_7mJuPfZsBzc3JkrANrFrcDqC
```

And link this environment variable to our constants file

_./src/env.constants.ts_:

```diff
export const envConstants = {
  NODE_ENV: process.env.NODE_ENV,
  PORT: process.env.PORT,
  STATIC_FILES_PATH: process.env.STATIC_FILES_PATH,
+ STRIPE_SECRET: process.env.STRIPE_SECRET,
};
```

Now, we are going to modify our API file and import Stripe library, making use of the Stripe secret we already have in our environment variables.

_./src/api.ts_:

```diff
import { Router } from 'express';
+ import Stripe from 'stripe';
+ import { envConstants } from './env.constants.js';

+ // https://github.com/stripe/stripe-node#usage-with-typescript
+ const stripe = new Stripe(envConstants.STRIPE_SECRET, {
+  apiVersion: null,
+ });

export const api = Router();

api.get('/', async (req, res) => {
  res.send({ id: '1', name: 'test data' });
});
```

The next step is to create an endpoint named `checkout`, which will be the one asking Stripe to create a new session. This endpoint will return a session id, which it will be forwarded to the client app. Few things to highlight here:

- Only card payments will be accepted (`payment_method_types`).
- Cart will be sent too (`line_items`).
- `mode` will be a one-time payment (It can be a subscription too).
- App flow will redirect to `success_url` if payment was successful.
- Othewise, app flow will be redirect to `cancel_url`.

```ts
api.post('/create-checkout-session', async (req, res) => {
  const session = await stripe.checkout.sessions.create({
    payment_method_types: ['card'],
    line_items: [
      {
        price_data: {
          currency: 'usd',
          product_data: {
            name: 'Stubborn Attachments',
            images: ['https://i.imgur.com/EHyR2nP.png'],
          },
          unit_amount: 2000,
        },
        quantity: 1,
      },
    ],
    mode: 'payment',
    success_url: `http://localhost:${envConstants.PORT}/success.html`,
    cancel_url: `http://localhost:${envConstants.PORT}/cancel.html`,
  });

  console.log("Session id:", session.id)
  console.log("Session URL:", session.url);
  res.redirect(303, session.url);
});
```

Let's define success and cancel pages:

_./public/success.html_:

```html
<html>
  <head>
    <title>Thanks for your order!</title>
    <link rel="stylesheet" href="styles.css" />
  </head>
  <body>
    <section>
      <p>
        We appreciate your business! If you have any questions, please email
        <a href="mailto:orders@example.com">orders@example.com</a>.
      </p>
    </section>
  </body>
</html>
```

_./public/cancel.html_:

```html
<html>
  <head>
    <title>Checkout canceled</title>
    <link rel="stylesheet" href="styles.css" />
  </head>
  <body>
    <section>
      <p>
        Forgot to add something to your cart? Shop around then come back to pay!
      </p>
    </section>
  </body>
</html>
```

Finally, we need to update the main page, in order to handle when _Checkout_ button is clicked.

- We will make use of Stripe SDK through CDN (It is the way recommended by Stripe, it is not recommended to add SDK to our bundle).
- When button is clicked, it will call `checkout` endpoint.
- Session id is retrieved.
- Client redirects app flow to Stripe payment gateway.
- Now we are in Stripe domain, where payment will be handle.
- After payment, Stripe will redirect flow to success or cancel page.

_./public/index.html_:

```diff
<!DOCTYPE html>
<html>
  <head>
    <title>Buy cool new product</title>
    <link rel="stylesheet" href="styles.css" />
    <script src="https://polyfill.io/v3/polyfill.min.js?version=3.52.1&features=fetch"></script>
+   <script src="https://js.stripe.com/v3/"></script>
  </head>
  <body>
    <section>
      <div class="product">
        <img
          src="https://i.imgur.com/EHyR2nP.png"
          alt="The cover of Stubborn Attachments"
        />
        <div class="description">
          <h3>Stubborn Attachments</h3>
          <h5>$20.00</h5>
        </div>
      </div>
+     <form action="/api/create-checkout-session" method="POST">
        <button id="checkout-button">Checkout</button>
+     </form>
    </section>
  </body>
</html>
```

Let's try out or app again.

```bash
npm start
```

Testing cards:

- Payment successful: 4242 4242 4242 4242
- Payment requires authentication: 4000 0025 0000 3155
- Payment denied: 4000 0000 0000 9995

More testing cards: https://stripe.com/docs/testing?testing-method=card-numbers

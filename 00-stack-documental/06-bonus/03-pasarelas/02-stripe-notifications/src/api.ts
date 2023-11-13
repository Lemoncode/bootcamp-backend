import { Router } from 'express';
import Stripe from 'stripe';
import { envConstants } from './env.constants.js';

export const api = Router();

const stripe = new Stripe(envConstants.STRIPE_SECRET);

api.get('/', async (req, res) => {
  res.send({ id: '1', name: 'test data' });
});

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

// Paste here signing secret given by _stripe listen --forward_ (it starts by por whsec_)
const signingSecret = "whsec_...";

api.post('/webhook', (req, res) => {
  const payload = req.body;
  const sig = req.headers['stripe-signature'];
  const rawBody = req['raw'];

  // Just show in the console what Stripe sent
  console.log(`Got payload: ${JSON.stringify(payload)}`);

  console.log(`Id: ${req.body.id}`);
  console.log(`Type: ${req.body.type}`);
  console.log(`Creation date: ${new Date(req.body.created)}`);
  console.log(`API version: ${req.body.api_version}`);

  try {
    // We use Stripe library to validate the response using the signingSecret.
    // This allows us to verify if the notification comes or not from Stripe.
    stripe.webhooks.constructEvent(rawBody, sig, signingSecret);
  } catch (err) {
    return res.status(400).send(`Webhook Error: ${err.message}`);
  }

  res.status(200).end();
});

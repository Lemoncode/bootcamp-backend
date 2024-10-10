// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  containerName: "alteregos",
  apiUrl: "http://localhost:5000/api/hero",
  appInsights: {
    instrumentationKey: "e0613572-c1f0-4665-89f9-1b9ab5946236"
  },
  storageUrl: "http://127.0.0.1:10000/devstoreaccount1",
  signalRUrl: "https://localhost:7238/messaging"
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.

// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
    production: false,
    apiUrl: 'https://localhost:44369/api',
    webUrl: 'http://localhost:4200/payment-confirmed',
    hubUrl: 'https://localhost:44369/hubs',
    flowUrl: 'https://sandbox.flow.cl/api',
    apyKey: '35AF82E6-E049-4BAB-943E-3F6L96AA8C2F',
    secretKey: '14f3cb31b3f2755071bf4d6e0c63ab11c1c7438d'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.

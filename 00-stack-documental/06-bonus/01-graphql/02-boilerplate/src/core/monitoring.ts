import { ENV } from "#core/constants/index.js";

if (ENV.IS_PRODUCTION) {
  import("newrelic");
}

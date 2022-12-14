import Transport from 'winston-transport';
import Rollbar, { Configuration } from 'rollbar';
import { MESSAGE } from 'triple-beam';

type Config = Transport.TransportStreamOptions & Configuration;

export class RollbarTransport extends Transport {
  private config: Config;
  private rollbar: Rollbar;

  constructor(config: Config = {}) {
    super(config);

    this.config = config;
    this.rollbar = new Rollbar({
      ...this.config,
    });
  }

  log(info, next) {
    setImmediate(() => this.emit('logged', info));
    const level = info.level;
    const message = info[MESSAGE];
    this.rollbar[level](message);
    next();
  }
}

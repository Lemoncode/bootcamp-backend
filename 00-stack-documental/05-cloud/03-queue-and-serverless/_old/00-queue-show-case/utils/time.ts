export const delay = (amount: number): Promise<void> =>
  new Promise((res) => {
    setTimeout(() => res(), amount);
  });

export const invokeWithDelay = (amount: number, cb: Function, args?: any): Promise<void> =>
  new Promise((resolve) => {
    setTimeout(() => { 
      (args) ? cb(...args) : cb(); 
      resolve();
    }, amount);
  });

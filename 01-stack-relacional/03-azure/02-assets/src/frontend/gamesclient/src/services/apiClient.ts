import axios, { AxiosBasicCredentials, AxiosRequestConfig, Method } from 'axios';
// import { store } from '../redux/store';

export type HTTPMethod = Method;

/**
 * Request base object
 */
export class Request {
    private config: AxiosRequestConfig;

    constructor(method: HTTPMethod, url: string) {
        this.config = {
            method: method,
            url: url,
        };
    }

    /**
     * Add a parameter to the request base URI
     *
     * @param {string} key name of the parameter
     * @param {string} value of the parameter
     * @returns {Request}
     */
    withQueryParameter(key: string, value: string): Request {
        this.config.params = { ...this.config.params, [key]: value };
        return this;
    }

    /**
     * Add a header key-value to the request
     *
     * @param {string} key name of the header
     * @param {string} value of the header
     * @returns
     */
    withHeader(key: string, value: any): Request {
        this.config.headers = { ...this.config.headers, [key]: value };
        return this;
    }

    withMultiPartHeaders(): Request {
        this.config.headers = { ...this.config.headers, Accept: '*/*' };
        this.config.headers = { ...this.config.headers, 'Content-Type': 'multipart/form-data' };
        return this;
    }

    /**
     * Configure basic authentication
     *
     * @param {AxiosBasicCredentials} basic crecdentials
     * @returns
     */
    withAuthentication(auth: AxiosBasicCredentials): Request {
        this.config.auth = auth;
        return this;
    }

    /**
     * Add a body to the request
     *
     * @param {any} body data of the request
     * @returns
     */
    withBody(body: any): Request {
        this.config.data = body;
        return this;
    }

    /**
     * Add a timeout value in milliseconds
     *
     * @param {number} milliseconds for waiting
     * @returns
     */
    withTimeout(millis: number): Request {
        this.config.timeout = millis;
        return this;
    }

    /**
     * Execute the type request
     *
     * @returns {Promise<T>} return a Promise with the type of the method
     */
    async execute<T>(): Promise<T> {
        const response = await axios.request(this.config);
        return response.data as T;
    }
    async executeBlob(): Promise<Blob> {
        const response = await axios.request({ ...this.config, responseType: 'blob' });
        return response.data;
    }
}

/**
 * Initialize the request
 * {@linkcode Request}
 * [[Request]]
 *
 * @param {HTTPMethod} method of our request
 * @param {string} url of the request
 * @returns {Request} an instance of Request
 */
export const apiFetch = (method: HTTPMethod, url: string): Request => {
    return new Request(method, url);
};

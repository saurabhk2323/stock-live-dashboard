import axios from "axios";

// create an axios instance
const apiClient = axios.create({
    baseURL: "http://localhost:5000",
    timeout: 10000,
});

// interceptor to add authentication token and headers
apiClient.interceptors.request.use(config => {
    // add additional headers if required
    if (config.url.includes('/privileged-endpoint')) {
        config.headers['X-Secret-Key'] = secretKey;
    }
    return config;
}, error => {
    return Promise.reject(error);
});


// interceptors to handle responses and errors
apiClient.interceptors.response.use(
    response => response,
    error => {
        if (error.response && error.response.data && error.response.data.message) {
            return Promise.reject(new Error(error.response.data.message));
        }
        return Promise.reject(error);
    }
)


export default apiClient;
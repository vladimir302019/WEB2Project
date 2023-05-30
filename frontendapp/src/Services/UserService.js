import {axiosClient} from "./AxiosService";

export const Login = async (requestBody) => {
    return await axiosClient.post(
        `${process.env.REACT_APP_API_ENDPOINT}/user/login`, requestBody
    );
};

export const Register = async (requestBody) => {
    return await axiosClient.put(
        `${process.env.REACT_APP_API_ENDPOINT}/user/register`, requestBody
    );
};
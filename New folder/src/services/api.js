import axios from 'axios';


const API_URL = 'https://localhost:7076';

export const getProducts = async () => {
    const response = await axios.get(`${API_URL}/catalog-api/products`);
    return response.data; // { products: [...] }
};

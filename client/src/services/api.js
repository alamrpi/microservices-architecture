import axios from 'axios';


const API_URL = 'http://localhost:5000';

export const getProducts = async () => {
    const response = await axios.get(`${API_URL}/catalog-api/products`);
    return response.data; // { products: [...] }
};

export const getBasket = async (userName) => {
    try {
        const response = await axios.get(`${API_URL}/basket-api/basket/${userName}`);
        return response.data;
    } catch (error) {
        return { userName, items: [] };
    }
};

export const updateBasket = async (basket) => {
    const response = await axios.post(`${API_URL}/basket-api/basket`, basket);
    return response.data;
};

export const deleteBasket = async (userName) => {
    await axios.delete(`${API_URL}/basket-api/basket/${userName}`);
};

// --- Checkout ---
export const checkoutBasket = async (checkoutData) => {
    const response = await axios.post(`${API_URL}/basket-api/basket/checkout`, {
        basketCheckoutDto: checkoutData
    });
    return response.data;
};
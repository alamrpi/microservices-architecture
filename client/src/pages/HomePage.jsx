import { useEffect, useState } from "react";
import { getProducts, getBasket, updateBasket } from "../services/api";

const HomePage = () => {
    const [products, setProducts] = useState([]);
    const USER_NAME = "swn"; //only for test

    useEffect(() => {
        loadProducts();
    }, []);

    const loadProducts = async () => {
        const data = await getProducts();
        console.log("API Data:", data);
        setProducts(data.products || []);
    };

    const handleAddToBasket = async (product) => {
        const basket = await getBasket(USER_NAME);

        const existingItem = basket.items.find(x => x.productId === product.id);

        if (existingItem) {
            existingItem.quantity += 1;
        } else {
            basket.items.push({
                productId: product.id,
                productName: product.name,
                price: product.price,
                quantity: 1,
                color: "Red"
            });
        }

        await updateBasket(basket);
        alert("Product added to basket!");
    };

    return (
        <div className="container mt-5">
            <h2>Catalog</h2>
            <div className="row">
                {products.map((p) => (
                    <div key={p.id} className="col-md-4 mb-4">
                        <div className="card h-100 shadow-sm">
                            <div className="card-body">
                                <h5 className="card-title">{p.name}</h5>
                                <p className="text-muted">{p.description}</p>
                                <h6 className="text-primary fw-bold">${p.price}</h6>
                                <button
                                    className="btn btn-primary w-100 mt-2"
                                    onClick={() => handleAddToBasket(p)}
                                >
                                    Add to Basket
                                </button>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default HomePage;
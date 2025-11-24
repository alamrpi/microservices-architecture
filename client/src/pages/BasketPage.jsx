import { useEffect, useState } from "react";
import { getBasket, deleteBasket } from "../services/api";
import { Link } from "react-router-dom";

const BasketPage = () => {
    const [basket, setBasket] = useState(null);
    const USER_NAME = "swn";

    useEffect(() => {
        loadBasket();
    }, []);

    const loadBasket = async () => {
        const data = await getBasket(USER_NAME);
        setBasket(data);
    };

    const handleRemove = async () => {
        await deleteBasket(USER_NAME);
        loadBasket(); // রিফ্রেশ
    };

    if (!basket || basket.items.length === 0) {
        return <div className="container mt-5">Your basket is empty.</div>;
    }

    // মোট দাম ক্যালকুলেট করা
    const totalPrice = basket.items.reduce((total, item) => total + (item.price * item.quantity), 0);

    return (
        <div className="container mt-5">
            <h2>Shopping Cart</h2>
            <table className="table table-striped mt-3">
                <thead>
                <tr>
                    <th>Product</th>
                    <th>Price</th>
                    <th>Quantity</th>
                    <th>Total</th>
                </tr>
                </thead>
                <tbody>
                {basket.items.map((item) => (
                    <tr key={item.productId}>
                        <td>{item.productName}</td>
                        <td>${item.price}</td>
                        <td>{item.quantity}</td>
                        <td>${item.price * item.quantity}</td>
                    </tr>
                ))}
                </tbody>
            </table>

            <div className="d-flex justify-content-between align-items-center">
                <h4>Total: ${totalPrice}</h4>
                <div>
                    <button className="btn btn-danger me-2" onClick={handleRemove}>Clear Basket</button>
                    {/* চেকআউট পেজে যাওয়ার লিঙ্ক */}
                    <Link to="/checkout" className="btn btn-success">Proceed to Checkout</Link>
                </div>
            </div>
        </div>
    );
};

export default BasketPage;
import { useState } from "react";
import { checkoutBasket } from "../services/api";
import { useNavigate } from "react-router-dom";

const CheckoutPage = () => {
    const navigate = useNavigate();
    const USER_NAME = "swn";

    const [formData, setFormData] = useState({
        userName: USER_NAME,
        firstName: "", lastName: "", emailAddress: "",
        addressLine: "", country: "", state: "", zipCode: "",
        cardName: "", cardNumber: "", expiration: "", cvv: "", paymentMethod: 1
    });

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await checkoutBasket(formData);
            alert("Order placed successfully!");
            navigate("/"); // হোম পেজে ফেরত পাঠাও
        } catch (error) {
            console.error(error);
            alert("Checkout failed!");
        }
    };

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    return (
        <div className="container mt-5 mb-5">
            <h2>Checkout</h2>
            <form onSubmit={handleSubmit}>
                <div className="row g-3">
                    <div className="col-md-6">
                        <label className="form-label">First Name</label>
                        <input type="text" className="form-control" name="firstName" onChange={handleChange} required />
                    </div>
                    <div className="col-md-6">
                        <label className="form-label">Last Name</label>
                        <input type="text" className="form-control" name="lastName" onChange={handleChange} required />
                    </div>
                    <div className="col-12">
                        <label className="form-label">Email</label>
                        <input type="email" className="form-control" name="emailAddress" onChange={handleChange} required />
                    </div>
                    <div className="col-12">
                        <label className="form-label">Address</label>
                        <input type="text" className="form-control" name="addressLine" onChange={handleChange} required />
                    </div>
                    {/* ... আরও ফিল্ড যোগ করতে পারেন (Card Info ইত্যাদি) ... */}
                </div>
                <button type="submit" className="btn btn-primary mt-4 w-100">Place Order</button>
            </form>
        </div>
    );
};

export default CheckoutPage;
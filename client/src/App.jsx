import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import HomePage from "./pages/HomePage";
import BasketPage from "./pages/BasketPage";
import CheckoutPage from "./pages/CheckoutPage";

function App() {
    return (
        <Router>
            <div>
                <nav className="navbar navbar-expand-lg navbar-dark bg-dark mb-4">
                    <div className="container">
                        <Link className="navbar-brand" to="/">E-Shop Global</Link>
                        <div className="navbar-nav ms-auto">
                            <Link className="nav-link" to="/">Home</Link>
                            <Link className="nav-link" to="/basket">Basket</Link>
                        </div>
                    </div>
                </nav>

                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/basket" element={<BasketPage />} />
                    <Route path="/checkout" element={<CheckoutPage />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
import { useEffect, useState } from "react";
import { getProducts } from "../services/api";
import ProductCard from "../components/ProductCard";

const HomePage = () => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadProducts();
    }, []);

    const loadProducts = async () => {
        try {
            const data = await getProducts();
            setProducts(data.products || data);
        } catch (error) {
            console.error("Error loading products:", error);
        } finally {
            setLoading(false);
        }
    };

    if (loading) return <div className="text-center mt-5">Loading...</div>;

    return (
        <div className="container mt-4">
            <h2>Catalog</h2>
            <div className="row">
                {products.map((p) => (
                    <div key={p.id} className="col-md-4 mb-4">
                        <ProductCard product={p} />
                    </div>
                ))}
            </div>
        </div>
    );
};

export default HomePage;
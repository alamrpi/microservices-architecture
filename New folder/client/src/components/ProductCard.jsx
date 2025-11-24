const ProductCard = ({ product }) => {
    return (
        <div className="card h-100 shadow-sm">
            <div className="card-body">
                <h5 className="card-title">{product.name}</h5>
                <p className="card-text text-muted">{product.description}</p>
                <h6 className="card-subtitle mb-2 text-primary">${product.price}</h6>
                <button className="btn btn-primary w-100 mt-2">
                    Add to Basket
                </button>
            </div>
        </div>
    );
};

export default ProductCard;
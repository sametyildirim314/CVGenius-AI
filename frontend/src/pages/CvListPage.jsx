import React, { useState } from "react";

function CvListPage() {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchCvs = () => {
        setLoading(true);
        setError(null);

        const apiUrl = `${process.env.REACT_APP_API_URL}/api/Cv`;
        console.log("Fetching CVs from:", apiUrl);

        fetch(apiUrl)
            .then((response) => {
                console.log("Response status:", response.status);
                
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                
                const contentType = response.headers.get("content-type");
                
                if (!contentType || !contentType.includes("application/json")) {
                    throw new Error(`Expected JSON but got ${contentType}`);
                }
                
                return response.json();
            })
            .then((data) => {
                console.log("✅ CVs received:", data);
                setData(data);
                setLoading(false);
            })
            .catch((error) => {
                console.error("❌ Error fetching CVs:", error);
                setError(error.message);
                setLoading(false);
            });
    };

    return (
        <div style={{ padding: "20px", maxWidth: "900px", margin: "0 auto" }}>
            <h1>📄 CV Listesi</h1>
            
            <button 
                onClick={fetchCvs}
                disabled={loading}
                style={{
                    padding: "10px 20px",
                    fontSize: "16px",
                    cursor: loading ? "not-allowed" : "pointer",
                    backgroundColor: loading ? "#ccc" : "#007bff",
                    color: "white",
                    border: "none",
                    borderRadius: "4px",
                    marginBottom: "20px"
                }}
            >
                {loading ? "Yükleniyor..." : "CV'leri Getir"}
            </button>

            {error && <p style={{ color: "red", marginBottom: "15px" }}>❌ Hata: {error}</p>}

            {data.length === 0 && !loading ? (
                <p style={{ color: "#666" }}>CV bulunmamaktadır.</p>
            ) : (
                <div>
                    <p style={{ marginBottom: "15px", fontWeight: "bold" }}>Toplam CV: {data.length}</p>
                    <ul style={{ listStyle: "none", padding: 0 }}>
                        {data.map((cv) => (
                            <li 
                                key={cv.id}
                                style={{
                                    padding: "15px",
                                    marginBottom: "10px",
                                    border: "1px solid #ddd",
                                    borderRadius: "4px",
                                    backgroundColor: "#f9f9f9"
                                }}
                            >
                                <div style={{ marginBottom: "5px" }}>
                                    <strong style={{ fontSize: "18px", color: "#333" }}>
                                        {cv.adSoyad}
                                    </strong>
                                    {cv.unvan && <span style={{ marginLeft: "10px", color: "#666" }}>({cv.unvan})</span>}
                                </div>
                                <div style={{ color: "#666", fontSize: "14px" }}>
                                    📧 {cv.email}
                                </div>
                                {cv.telefon && <div style={{ color: "#666", fontSize: "14px" }}>📞 {cv.telefon}</div>}
                                {cv.adres && <div style={{ color: "#666", fontSize: "14px" }}>📍 {cv.adres}</div>}
                            </li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
}

export default CvListPage;
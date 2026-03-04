import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import './App.css';
import CvListPage from "./pages/CvListPage";
import CreateCvPage from "./pages/CreateCvPage";

function App() {
    return (
        <Router>
            <div style={{ minHeight: "100vh", backgroundColor: "#f5f5f5" }}>
                {/* Navigation Bar */}
                <nav style={{
                    backgroundColor: "#333",
                    padding: "15px 20px",
                    boxShadow: "0 2px 4px rgba(0,0,0,0.1)"
                }}>
                    <div style={{ maxWidth: "900px", margin: "0 auto", display: "flex", gap: "20px" }}>
                        <h2 style={{ color: "white", margin: 0, marginRight: "auto" }}>
                            📄 CV Yönetim Sistemi
                        </h2>
                        <Link 
                            to="/" 
                            style={{
                                color: "white",
                                textDecoration: "none",
                                padding: "10px 15px",
                                borderRadius: "4px",
                                backgroundColor: "transparent",
                                transition: "background-color 0.3s"
                            }}
                            onMouseEnter={(e) => e.target.style.backgroundColor = "#555"}
                            onMouseLeave={(e) => e.target.style.backgroundColor = "transparent"}
                        >
                            📋 CV'leri Listele
                        </Link>
                        <Link 
                            to="/create" 
                            style={{
                                color: "white",
                                textDecoration: "none",
                                padding: "10px 15px",
                                borderRadius: "4px",
                                backgroundColor: "transparent",
                                transition: "background-color 0.3s"
                            }}
                            onMouseEnter={(e) => e.target.style.backgroundColor = "#555"}
                            onMouseLeave={(e) => e.target.style.backgroundColor = "transparent"}
                        >
                            ➕ CV Oluştur
                        </Link>
                    </div>
                </nav>

                {/* Main Content */}
                <Routes>
                    <Route path="/" element={<CvListPage />} />
                    <Route path="/create" element={<CreateCvPage />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;

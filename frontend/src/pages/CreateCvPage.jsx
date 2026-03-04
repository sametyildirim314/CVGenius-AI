import React, { useState } from "react";

function CreateCvPage() {
    const [users, setUsers] = useState([]);
    const [loadingUsers, setLoadingUsers] = useState(false);
    const [error, setError] = useState(null);
    const [selectedUserId, setSelectedUserId] = useState("");
    const [cvForm, setCvForm] = useState({
        adSoyad: "",
        unvan: "",
        email: "",
        telefon: "",
        adres: "",
        özet: "",
        yetenekler: ""
    });
    const [creating, setCreating] = useState(false);

    // Kullanıcıları getir
    const fetchUsers = () => {
        setLoadingUsers(true);
        setError(null);

        const apiUrl = `${process.env.REACT_APP_API_URL}/api/Cv/kullanicigetir`;
        console.log("Fetching users from:", apiUrl);

        fetch(apiUrl)
            .then((response) => {
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                return response.json();
            })
            .then((data) => {
                console.log("Users received:", data);
                setUsers(data);
                setLoadingUsers(false);
            })
            .catch((error) => {
                console.error("Error fetching users:", error);
                setError(error.message);
                setLoadingUsers(false);
            });
    };

    // Form input değişikliği
    const handleFormChange = (e) => {
        const { name, value } = e.target;
        setCvForm({
            ...cvForm,
            [name]: value
        });
    };

    // CV Oluştur ve İndir
    const handleCreateCv = async (e) => {
        e.preventDefault();

        if (!selectedUserId) {
            setError("Lütfen bir kullanıcı seçin");
            return;
        }

        if (!cvForm.adSoyad || !cvForm.email) {
            setError("Ad Soyad ve Email zorunludur");
            return;
        }

        setCreating(true);
        setError(null);

        try {
            const cvData = {
                adSoyad: cvForm.adSoyad,
                unvan: cvForm.unvan,
                email: cvForm.email,
                telefon: cvForm.telefon,
                adres: cvForm.adres,
                özet: cvForm.özet,
                yetenekler: cvForm.yetenekler
                    ? cvForm.yetenekler.split(",").map(y => y.trim())
                    : []
            };

            const apiUrl = `${process.env.REACT_APP_API_URL}/api/Cv/kullanici/${selectedUserId}/olustur-ve-indir`;
            
            console.log("Creating CV for user:", selectedUserId);

            const response = await fetch(apiUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(cvData)
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            // PDF dosyasını indir
            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement("a");
            link.href = url;
            link.download = `cv-${cvForm.adSoyad.replace(" ", "_")}.pdf`;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            window.URL.revokeObjectURL(url);

            // Formu sıfırla
            setCvForm({
                adSoyad: "",
                unvan: "",
                email: "",
                telefon: "",
                adres: "",
                özet: "",
                yetenekler: ""
            });

            setError(null);
            alert("✅ CV başarıyla oluşturuldu ve indirildi!");
        } catch (error) {
            console.error("Error creating CV:", error);
            setError("CV oluşturulurken hata oluştu: " + error.message);
        } finally {
            setCreating(false);
        }
    };

    return (
        <div style={{ padding: "20px", maxWidth: "800px", margin: "0 auto" }}>
            <h1>📝 CV Oluşturucu</h1>

            {/* Kullanıcı Getirme Bölümü */}
            <div style={{ marginBottom: "30px" }}>
                <button
                    onClick={fetchUsers}
                    disabled={loadingUsers}
                    style={{
                        padding: "10px 20px",
                        fontSize: "16px",
                        cursor: loadingUsers ? "not-allowed" : "pointer",
                        backgroundColor: loadingUsers ? "#ccc" : "#007bff",
                        color: "white",
                        border: "none",
                        borderRadius: "4px"
                    }}
                >
                    {loadingUsers ? "Yükleniyor..." : "Kullanıcıları Yükle"}
                </button>
            </div>

            {error && <p style={{ color: "red", marginBottom: "15px" }}>❌ {error}</p>}

            {/* Kullanıcı Seçimi */}
            {users.length > 0 && (
                <div style={{ marginBottom: "30px" }}>
                    <label style={{ display: "block", marginBottom: "10px", fontWeight: "bold" }}>
                        Kullanıcı Seç:
                    </label>
                    <select
                        value={selectedUserId}
                        onChange={(e) => setSelectedUserId(e.target.value)}
                        style={{
                            width: "100%",
                            padding: "10px",
                            fontSize: "14px",
                            borderRadius: "4px",
                            border: "1px solid #ddd"
                        }}
                    >
                        <option value="">-- Seçiniz --</option>
                        {users.map((user) => (
                            <option key={user.id} value={user.id}>
                                {user.ad} {user.soyad} ({user.email})
                            </option>
                        ))}
                    </select>
                </div>
            )}

            {/* CV Formu */}
            {selectedUserId && (
                <form onSubmit={handleCreateCv} style={{ border: "1px solid #ddd", padding: "20px", borderRadius: "4px" }}>
                    <h2>CV Bilgilerini Girin</h2>

                    <div style={{ marginBottom: "15px" }}>
                        <label style={{ display: "block", marginBottom: "5px", fontWeight: "bold" }}>
                            Ad Soyad *
                        </label>
                        <input
                            type="text"
                            name="adSoyad"
                            value={cvForm.adSoyad}
                            onChange={handleFormChange}
                            required
                            style={{
                                width: "100%",
                                padding: "8px",
                                borderRadius: "4px",
                                border: "1px solid #ddd",
                                boxSizing: "border-box"
                            }}
                        />
                    </div>

                    <div style={{ marginBottom: "15px" }}>
                        <label style={{ display: "block", marginBottom: "5px", fontWeight: "bold" }}>
                            Ünvan
                        </label>
                        <input
                            type="text"
                            name="unvan"
                            value={cvForm.unvan}
                            onChange={handleFormChange}
                            placeholder="Örn: Yazılım Geliştirici"
                            style={{
                                width: "100%",
                                padding: "8px",
                                borderRadius: "4px",
                                border: "1px solid #ddd",
                                boxSizing: "border-box"
                            }}
                        />
                    </div>

                    <div style={{ marginBottom: "15px" }}>
                        <label style={{ display: "block", marginBottom: "5px", fontWeight: "bold" }}>
                            Email *
                        </label>
                        <input
                            type="email"
                            name="email"
                            value={cvForm.email}
                            onChange={handleFormChange}
                            required
                            style={{
                                width: "100%",
                                padding: "8px",
                                borderRadius: "4px",
                                border: "1px solid #ddd",
                                boxSizing: "border-box"
                            }}
                        />
                    </div>

                    <div style={{ marginBottom: "15px" }}>
                        <label style={{ display: "block", marginBottom: "5px", fontWeight: "bold" }}>
                            Telefon
                        </label>
                        <input
                            type="tel"
                            name="telefon"
                            value={cvForm.telefon}
                            onChange={handleFormChange}
                            placeholder="Örn: 0555 555 55 55"
                            style={{
                                width: "100%",
                                padding: "8px",
                                borderRadius: "4px",
                                border: "1px solid #ddd",
                                boxSizing: "border-box"
                            }}
                        />
                    </div>

                    <div style={{ marginBottom: "15px" }}>
                        <label style={{ display: "block", marginBottom: "5px", fontWeight: "bold" }}>
                            Adres
                        </label>
                        <input
                            type="text"
                            name="adres"
                            value={cvForm.adres}
                            onChange={handleFormChange}
                            placeholder="Örn: İstanbul, Türkiye"
                            style={{
                                width: "100%",
                                padding: "8px",
                                borderRadius: "4px",
                                border: "1px solid #ddd",
                                boxSizing: "border-box"
                            }}
                        />
                    </div>

                    <div style={{ marginBottom: "15px" }}>
                        <label style={{ display: "block", marginBottom: "5px", fontWeight: "bold" }}>
                            Özet
                        </label>
                        <textarea
                            name="özet"
                            value={cvForm.özet}
                            onChange={handleFormChange}
                            placeholder="Kısa bir özet yazın..."
                            rows="4"
                            style={{
                                width: "100%",
                                padding: "8px",
                                borderRadius: "4px",
                                border: "1px solid #ddd",
                                boxSizing: "border-box",
                                fontFamily: "Arial"
                            }}
                        />
                    </div>

                    <div style={{ marginBottom: "15px" }}>
                        <label style={{ display: "block", marginBottom: "5px", fontWeight: "bold" }}>
                            Yetenekler (virgülle ayırın)
                        </label>
                        <input
                            type="text"
                            name="yetenekler"
                            value={cvForm.yetenekler}
                            onChange={handleFormChange}
                            placeholder="Örn: React, C#, SQL, JavaScript"
                            style={{
                                width: "100%",
                                padding: "8px",
                                borderRadius: "4px",
                                border: "1px solid #ddd",
                                boxSizing: "border-box"
                            }}
                        />
                    </div>

                    <button
                        type="submit"
                        disabled={creating}
                        style={{
                            width: "100%",
                            padding: "12px",
                            fontSize: "16px",
                            fontWeight: "bold",
                            cursor: creating ? "not-allowed" : "pointer",
                            backgroundColor: creating ? "#ccc" : "#28a745",
                            color: "white",
                            border: "none",
                            borderRadius: "4px"
                        }}
                    >
                        {creating ? "CV Oluşturuluyor..." : "📥 CV Oluştur ve İndir"}
                    </button>
                </form>
            )}
        </div>
    );
}

export default CreateCvPage;
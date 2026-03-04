import React, { useState, useEffect, useMemo } from "react";

const API = process.env.REACT_APP_API_URL;

function CvListPage() {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [search, setSearch] = useState("");
    const [expandedId, setExpandedId] = useState(null);

    useEffect(() => {
        fetchCvs();
    }, []);

    const fetchCvs = () => {
        setLoading(true);
        setError(null);

        fetch(`${API}/api/Cv`)
            .then((res) => {
                if (!res.ok) throw new Error(`Sunucu hatası: ${res.status}`);
                const ct = res.headers.get("content-type");
                if (!ct || !ct.includes("application/json"))
                    throw new Error("Beklenmeyen yanıt formatı");
                return res.json();
            })
            .then((d) => {
                setData(d);
                setLoading(false);
            })
            .catch((err) => {
                setError(err.message);
                setLoading(false);
            });
    };

    const filtered = useMemo(() => {
        if (!search.trim()) return data;
        const q = search.toLowerCase();
        return data.filter(
            (cv) =>
                cv.adSoyad?.toLowerCase().includes(q) ||
                cv.unvan?.toLowerCase().includes(q) ||
                cv.email?.toLowerCase().includes(q)
        );
    }, [data, search]);

    const getInitials = (name) => {
        if (!name) return "?";
        return name
            .split(" ")
            .map((w) => w[0])
            .join("")
            .toUpperCase()
            .slice(0, 2);
    };

    const formatDate = (dateStr) => {
        if (!dateStr) return "";
        try {
            return new Date(dateStr).toLocaleDateString("tr-TR", {
                day: "numeric",
                month: "long",
                year: "numeric",
            });
        } catch {
            return dateStr;
        }
    };

    const userCount = useMemo(() => {
        const unique = new Set(data.map((cv) => cv.kullaniciId));
        return unique.size;
    }, [data]);

    return (
        <div>
            <div className="page-header">
                <h1>CV Listesi</h1>
                <p>Sisteme kayıtlı tüm CV'leri görüntüleyin ve yönetin</p>
            </div>

            {/* Stats */}
            <div className="stats-bar">
                <div className="stat-card">
                    <div className="stat-icon blue">
                        <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                            <path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z" />
                            <polyline points="14 2 14 8 20 8" />
                        </svg>
                    </div>
                    <div>
                        <div className="stat-value">{loading ? "-" : data.length}</div>
                        <div className="stat-label">Toplam CV</div>
                    </div>
                </div>
                <div className="stat-card">
                    <div className="stat-icon green">
                        <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                            <path d="M17 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2" />
                            <circle cx="9" cy="7" r="4" />
                            <path d="M23 21v-2a4 4 0 00-3-3.87" />
                            <path d="M16 3.13a4 4 0 010 7.75" />
                        </svg>
                    </div>
                    <div>
                        <div className="stat-value">{loading ? "-" : userCount}</div>
                        <div className="stat-label">Kullanıcı</div>
                    </div>
                </div>
            </div>

            {/* Search + Refresh */}
            <div style={{ display: "flex", gap: "12px", marginBottom: "24px", alignItems: "stretch" }}>
                <div className="search-bar" style={{ flex: 1, marginBottom: 0 }}>
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                        <circle cx="11" cy="11" r="8" />
                        <line x1="21" y1="21" x2="16.65" y2="16.65" />
                    </svg>
                    <input
                        type="text"
                        placeholder="Ad, unvan veya e-posta ile arayın..."
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                    />
                </div>
                <button className="btn btn-secondary" onClick={fetchCvs} disabled={loading} title="Yenile">
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" style={loading ? { animation: "spin 1s linear infinite" } : {}}>
                        <polyline points="23 4 23 10 17 10" />
                        <polyline points="1 20 1 14 7 14" />
                        <path d="M3.51 9a9 9 0 0114.85-3.36L23 10M1 14l4.64 4.36A9 9 0 0020.49 15" />
                    </svg>
                </button>
            </div>

            {/* Error */}
            {error && (
                <div className="alert alert-error">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                        <circle cx="12" cy="12" r="10" />
                        <line x1="12" y1="8" x2="12" y2="12" />
                        <line x1="12" y1="16" x2="12.01" y2="16" />
                    </svg>
                    <div>
                        <strong>Bağlantı Hatası:</strong> {error}
                        <div style={{ marginTop: "6px" }}>
                            <button className="btn btn-sm btn-danger" onClick={fetchCvs}>
                                Tekrar Dene
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {/* Loading Skeletons */}
            {loading && (
                <div className="cv-grid">
                    {[1, 2, 3, 4, 5, 6].map((i) => (
                        <div key={i} className="cv-card" style={{ opacity: 0.5 }}>
                            <div className="cv-card-header">
                                <div
                                    className="loading-skeleton"
                                    style={{ width: 48, height: 48, borderRadius: "50%" }}
                                />
                                <div style={{ flex: 1 }}>
                                    <div className="loading-skeleton" style={{ height: 18, width: "70%", marginBottom: 8 }} />
                                    <div className="loading-skeleton" style={{ height: 14, width: "45%" }} />
                                </div>
                            </div>
                            <div style={{ borderTop: "1px solid var(--color-border)", paddingTop: 12, marginTop: 4 }}>
                                <div className="loading-skeleton" style={{ height: 14, width: "80%", marginBottom: 8 }} />
                                <div className="loading-skeleton" style={{ height: 14, width: "60%" }} />
                            </div>
                        </div>
                    ))}
                </div>
            )}

            {/* Empty State */}
            {!loading && !error && filtered.length === 0 && (
                <div className="empty-state">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round">
                        <path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z" />
                        <polyline points="14 2 14 8 20 8" />
                    </svg>
                    <h3>{search ? "Sonuç bulunamadı" : "Henüz CV bulunmuyor"}</h3>
                    <p>
                        {search
                            ? `"${search}" ile eşleşen CV bulunamadı.`
                            : "Yeni bir CV oluşturmak için \"CV Oluştur\" sayfasını kullanın."}
                    </p>
                </div>
            )}

            {/* CV Grid */}
            {!loading && filtered.length > 0 && (
                <>
                    {search && (
                        <p style={{ marginBottom: "16px", fontSize: "0.88rem", color: "var(--color-text-secondary)" }}>
                            <strong>{filtered.length}</strong> sonuç bulundu
                        </p>
                    )}
                    <div className="cv-grid">
                        {filtered.map((cv, index) => (
                            <div
                                key={cv.id}
                                className="cv-card"
                                style={{ animationDelay: `${index * 0.05}s` }}
                                onClick={() => setExpandedId(expandedId === cv.id ? null : cv.id)}
                            >
                                <div className="cv-card-header">
                                    <div className="cv-card-avatar">{getInitials(cv.adSoyad)}</div>
                                    <div style={{ flex: 1, minWidth: 0 }}>
                                        <div className="cv-card-name">{cv.adSoyad}</div>
                                        {cv.unvan && <div className="cv-card-title">{cv.unvan}</div>}
                                    </div>
                                </div>

                                <div className="cv-card-info">
                                    {cv.email && (
                                        <div className="cv-card-info-item">
                                            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                                                <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z" />
                                                <polyline points="22,6 12,13 2,6" />
                                            </svg>
                                            <span style={{ overflow: "hidden", textOverflow: "ellipsis", whiteSpace: "nowrap" }}>{cv.email}</span>
                                        </div>
                                    )}
                                    {cv.telefon && (
                                        <div className="cv-card-info-item">
                                            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                                                <path d="M22 16.92v3a2 2 0 01-2.18 2 19.79 19.79 0 01-8.63-3.07 19.5 19.5 0 01-6-6 19.79 19.79 0 01-3.07-8.67A2 2 0 014.11 2h3a2 2 0 012 1.72 12.84 12.84 0 00.7 2.81 2 2 0 01-.45 2.11L8.09 9.91a16 16 0 006 6l1.27-1.27a2 2 0 012.11-.45 12.84 12.84 0 002.81.7A2 2 0 0122 16.92z" />
                                            </svg>
                                            {cv.telefon}
                                        </div>
                                    )}
                                    {cv.adres && (
                                        <div className="cv-card-info-item">
                                            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                                                <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0118 0z" />
                                                <circle cx="12" cy="10" r="3" />
                                            </svg>
                                            {cv.adres}
                                        </div>
                                    )}
                                </div>

                                {/* Expanded Details */}
                                {expandedId === cv.id && (
                                    <div style={{ marginTop: 14, paddingTop: 14, borderTop: "1px solid var(--color-border)", animation: "fadeIn 0.3s ease" }}>
                                        {cv.özet && (
                                            <div style={{ marginBottom: 12 }}>
                                                <div style={{ fontSize: "0.78rem", fontWeight: 700, color: "var(--color-text-muted)", textTransform: "uppercase", letterSpacing: "0.05em", marginBottom: 4 }}>Özet</div>
                                                <p style={{ fontSize: "0.85rem", color: "var(--color-text-secondary)", lineHeight: 1.6 }}>{cv.özet}</p>
                                            </div>
                                        )}
                                        {cv.yetenekler && cv.yetenekler.length > 0 && (
                                            <div style={{ marginBottom: 12 }}>
                                                <div style={{ fontSize: "0.78rem", fontWeight: 700, color: "var(--color-text-muted)", textTransform: "uppercase", letterSpacing: "0.05em", marginBottom: 6 }}>Yetenekler</div>
                                                <div style={{ display: "flex", flexWrap: "wrap", gap: 6 }}>
                                                    {cv.yetenekler.map((y, i) => (
                                                        <span key={i} className="badge badge-primary">{y}</span>
                                                    ))}
                                                </div>
                                            </div>
                                        )}
                                        {cv.deneyim && cv.deneyim.length > 0 && (
                                            <div style={{ marginBottom: 12 }}>
                                                <div style={{ fontSize: "0.78rem", fontWeight: 700, color: "var(--color-text-muted)", textTransform: "uppercase", letterSpacing: "0.05em", marginBottom: 4 }}>Deneyim</div>
                                                {cv.deneyim.map((d, i) => (
                                                    <div key={i} style={{ fontSize: "0.85rem", color: "var(--color-text-secondary)", marginBottom: 4 }}>
                                                        <strong>{d.pozisyon}</strong> - {d.sirket} ({d.baslangicTarihi} - {d.bitisTarihi || "Devam"})
                                                    </div>
                                                ))}
                                            </div>
                                        )}
                                    </div>
                                )}

                                {cv.olusturulmaTarihi && (
                                    <div className="cv-card-date">
                                        <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                                            <circle cx="12" cy="12" r="10" />
                                            <polyline points="12 6 12 12 16 14" />
                                        </svg>
                                        {formatDate(cv.olusturulmaTarihi)}
                                    </div>
                                )}
                            </div>
                        ))}
                    </div>
                </>
            )}
        </div>
    );
}

export default CvListPage;

<%@ page title="About Car Rental" language="C#" masterpagefile="~/MasterPageVisitor.master" autoeventwireup="true" inherits="AboutUs, App_Web_pskrdzto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="about-container">
        <div class="about-card fade-in">
            <h2 class="form-title">About Car Rental</h2>

            <div class="about-content">
                <p class="about-text">
                    Find a car for your journey by comparing vehicles, prices and specifications in our rental catalog.
                </p>

                <p class="about-text sub-text">
                    Choose your dates, check availability and send a rental request for approval. Payment takes place outside the website.
                </p>

                <div class="features-list">
                    <div class="feature-item">
                        <i class="fas fa-car"></i>
                        <div class="feature-info">
                            <h4>Explore the Fleet</h4>
                            <p>Compare vehicle vehicleCategories, specifications and customer reviews.</p>
                        </div>
                    </div>
                    <div class="feature-item">
                        <i class="fas fa-calendar-alt"></i>
                        <div class="feature-info">
                            <h4>Check Availability</h4>
                            <p>Select pickup and return dates before sending a rental request.</p>
                        </div>
                    </div>
                    <div class="feature-item">
                        <i class="fas fa-map-marker-alt"></i>
                        <div class="feature-info">
                            <h4>Find a Branch</h4>
                            <p>See the branch associated with each car in the catalog.</p>
                        </div>
                    </div>
                </div>

                <div class="mission-section">
                    <h3 class="section-subtitle">Plan Your Journey</h3>
                    <p class="about-text quote">
                        Compare cars and follow your rental requests from your account.
                    </p>
                </div>
            </div>

            <div class="signin-section">
                <p class="already-account">Ready to find a car?</p>
                <asp:Button ID="btnRegisterLink" runat="server" Text="Create an Account" CssClass="submit-button" OnClientClick="window.location.href='RegisterVisitor.aspx'; return false;" />
            </div>
        </div>
    </div>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css">

    <style>
        .about-container {
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            padding: 40px 20px;
            background: transparent;
        }

        .about-card {
            width: 100%;
            max-width: 650px;
            background: linear-gradient(135deg, #23232b 0%, #2d2d38 100%);
            padding: 45px;
            border-radius: 20px;
            box-shadow: 0 10px 40px rgba(124, 58, 237, 0.25);
            border: 1px solid rgba(168, 85, 247, 0.2);
        }

        .form-title {
            text-align: center;
            font-size: 32px;
            font-weight: 800;
            margin-bottom: 25px;
            background: linear-gradient(45deg, #a855f7, #7c3aed);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            text-transform: uppercase;
            letter-spacing: 2px;
        }

        .about-text {
            color: #f1f1f5;
            line-height: 1.6;
            font-size: 16px;
            text-align: center;
            margin-bottom: 15px;
        }

        .sub-text { color: #94a3b8; font-size: 14px; }
        .highlight { color: #c4b5fd; font-weight: bold; }
        .quote { font-style: italic; color: #a855f7; font-size: 15px; padding: 0 20px; }

        .features-list {
            margin: 30px 0;
            background: rgba(15, 15, 20, 0.4);
            border-radius: 15px;
            padding: 25px;
            border: 1px solid rgba(124, 58, 237, 0.1);
        }

        .feature-item { display: flex; align-items: flex-start; gap: 20px; margin-bottom: 25px; }
        .feature-item:last-child { margin-bottom: 0; }
        .feature-item i { color: #a855f7; font-size: 24px; min-width: 40px; text-align: center; padding-top: 5px; }

        .feature-info h4 { color: #e9d5ff; font-size: 17px; margin: 0 0 5px 0; font-weight: 700; }
        .feature-info p { color: #94a3b8; font-size: 14px; margin: 0; line-height: 1.5; }

        .section-subtitle {
            color: #e9d5ff;
            font-size: 18px;
            text-align: center;
            margin: 30px 0 15px 0;
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        /* Submit Button Style - Applied to the Join button */
        .submit-button {
            width: 100%;
            padding: 14px;
            background: linear-gradient(45deg, #00aaff 0%, #9900ff 100%);
            border: none;
            border-radius: 10px;
            color: white;
            font-weight: bold;
            font-size: 16px;
            cursor: pointer;
            box-shadow: 0 4px 15px rgba(124, 58, 237, 0.3);
            transition: 0.3s;
            margin-top: 10px;
            text-transform: uppercase;
        }

        .submit-button:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(124, 58, 237, 0.5);
        }

        .signin-section {
            margin-top: 30px;
            text-align: center;
            border-top: 1px solid rgba(255,255,255,0.1);
            padding-top: 25px;
        }

        .already-account { color: #94a3b8; font-size: 14px; margin-bottom: 15px; }

        @keyframes fadeInUp {
            from { opacity: 0; transform: translateY(20px); }
            to { opacity: 1; transform: translateY(0); }
        }

        .fade-in { animation: fadeInUp 0.6s ease-out forwards; }
    </style>
</asp:Content>
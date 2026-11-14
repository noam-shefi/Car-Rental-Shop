<%@ page title="" language="C#" masterpagefile="~/MasterPageVisitor.master" autoeventwireup="true" inherits="home, App_Web_53vxbbw0" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<style>
.big-title {
    font-family: 'Inter', sans-serif;
    font-weight: 700;
    text-align: center;
    color: white;
}

.gradient-text {
    background: linear-gradient(90deg,#4facfe,#00f2fe,#7c3aed);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
}

.gradient-text-buttom {
    background: none;
    -webkit-background-clip: unset;
    -webkit-text-fill-color: white; 
}

.gradient-text-buttom .cta-button-black {
    color: black !important;
    -webkit-text-fill-color: unset !important; 
}

.section-bg-dark {
    background-color: #111827;
    color: white;
}

.section-bg-blue {
    background-color: #0f172a;
    color: white;
}

.text-secondary-light {
    color: #cbd5e1;
}

.card-light {
    background-color: #2c2f38; 
    padding: 1.2rem;
    border-radius: 0.75rem;
    box-shadow: 0 1px 3px rgba(0,0,0,0.1);
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    text-align: center;
    flex: 1 1 22%; 
    margin: 0.5rem;
    min-width: 220px; 
}

.row-center {
    display: flex;
    justify-content: center;
    flex-wrap: wrap;
    gap: 1rem;
}

.stats-card {
    background: none;
    flex: 1 1 22%;
    min-width: 180px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    text-align: center;
    margin: 0.5rem;
}

.py-5 { padding-top: 3rem; padding-bottom: 3rem; }
.mb-5 { margin-bottom: 3rem; }
</style>


<section class="py-5 section-bg-blue">
    <div class="container py-5 text-center">
        <h1 class="big-title gradient-text" style="font-size: 3rem;">
            מוצאים את הרכב המתאים לכל נסיעה
        </h1>
        <p class="big-title mb-4" style="font-size: 1.5rem;">
            השוו רכבים, בדקו זמינות לפי תאריכים ושלחו בקשת השכרה.
        </p>

        <div class="mt-4">
            <a href="Cars.aspx" class="btn gradient-btn me-3 px-4 py-2">לצפייה ברכבים</a>
            <a href="CarMatch.aspx" class="btn btn-outline-light px-4 py-2">התאמת רכב לטיול</a>
        </div>

        <p class="mt-3 big-title text-secondary-light" style="font-size: 1rem;">
            מחירים יומיים ברורים • בחירת תאריכים גמישה • בקשת השכרה ללא תשלום באתר
        </p>
    </div>
</section>

<section class="py-5 section-bg-dark">
    <div class="container text-center">
        <h2 class="big-title gradient-text mb-5" style="font-size: 2.5rem;">השכרת רכב בדרך שלכם</h2>
        <div class="row-center">
            <div class="card-light">
                <h5 class="big-title gradient-text" style="font-size: 1.25rem;">מבחר רכבים</h5>
                <p class="big-title text-secondary-light mt-2" style="font-size: 1rem;">
                    בחרו בין רכבי סדאן, משפחתיים, רכבי שטח ורכבי ספורט.
                </p>
            </div>
            <div class="card-light">
                <h5 class="big-title gradient-text" style="font-size: 1.25rem;">זמינות לפי תאריכים</h5>
                <p class="big-title text-secondary-light mt-2" style="font-size: 1rem;">
                    בדקו תאריכי איסוף והחזרה לפני שליחת בקשת ההשכרה.
                </p>
            </div>
            <div class="card-light">
                <h5 class="big-title gradient-text" style="font-size: 1.25rem;">הזמנה למעקב</h5>
                <p class="big-title text-secondary-light mt-2" style="font-size: 1rem;">
                    כל בקשה נשלחת לאישור ותוכלו לעקוב אחר הסטטוס בחשבון.
                </p>
            </div>
            <div class="card-light">
                <h5 class="big-title gradient-text" style="font-size: 1.25rem;">כיסוי ושירות</h5>
                <p class="big-title text-secondary-light mt-2" style="font-size: 1rem;">
                    המידע על הביטוח והשירות מוצג לצורכי שיווק; התשלום מתבצע מחוץ לאתר.
                </p>
            </div>
        </div>
    </div>
</section>

<section class="py-5 text-white text-center section-bg-blue">
    <div class="container">
        <h2 class="big-title gradient-text mb-3" style="font-size: 2rem;">נוסעים בראש שקט</h2>
        <p class="big-title text-secondary-light mb-5" style="font-size: 1.1rem;">
            בחרו רכב לפי המסלול, מספר הנוסעים והציוד שאתם מתכננים לקחת.
        </p>

        <div class="row-center">
            <div class="card-light">
                <h5 class="big-title gradient-text" style="font-size: 1.25rem;">🚗 מבחר רכבים</h5>
                <p class="big-title text-secondary-light mt-2" style="font-size: 1rem;">
                    רכבים עם מפרטים טכניים, דירוגים ומיקום הסניף.
                </p>
            </div>
            <div class="card-light">
                <h5 class="big-title gradient-text" style="font-size: 1.25rem;">📅 בדיקת תאריכים</h5>
                <p class="big-title text-secondary-light mt-2" style="font-size: 1rem;">
                    בדקו זמינות לפי תאריכי האיסוף וההחזרה שבחרתם.
                </p>
            </div>
            <div class="card-light">
                <h5 class="big-title gradient-text" style="font-size: 1.25rem;">⭐ דירוגים וביקורות</h5>
                <p class="big-title text-secondary-light mt-2" style="font-size: 1rem;">
                    השוו חוות דעת ומפרטים לפני שליחת בקשת ההשכרה.
                </p>
            </div>
            <div class="card-light">
                <h5 class="big-title gradient-text" style="font-size: 1.25rem;">🧭 המלצה לפי הטיול</h5>
                <p class="big-title text-secondary-light mt-2" style="font-size: 1rem;">
                    תארו את הטיול וקבלו התאמה לרכב מתוך הצי הזמין.
                </p>
            </div>
        </div>
    </div>
</section>

<section class="py-5 section-bg-dark">
    <div class="container">
        <div class="row-center">
            <div class="stats-card"><h2 class="big-title gradient-text">4</h2><p class="big-title text-secondary-light">קטגוריות רכב</p></div>
            <div class="stats-card"><h2 class="big-title gradient-text">₪</h2><p class="big-title text-secondary-light">מחיר יומי לכל רכב</p></div>
            <div class="stats-card"><h2 class="big-title gradient-text">★</h2><p class="big-title text-secondary-light">דירוגים וביקורות לקוחות</p></div>
            <div class="stats-card"><h2 class="big-title gradient-text">✓</h2><p class="big-title text-secondary-light">בקשות השכרה במעקב אישי</p></div>
        </div>
    </div>
</section>

<section class="py-5 text-center gradient-text-buttom" style="background: linear-gradient(90deg,#4facfe,#00f2fe,#7c3aed);">
    <div class="container">
        <h2 class="big-title mb-3" style="font-size: 2.5rem; color: white;">מוכנים לצאת לדרך?</h2>
        <p class="big-title mb-4" style="font-size: 1.5rem; color: white;">
            בחרו רכב ובדקו זמינות לפי תאריכי הנסיעה.
        </p>
        <a href="Cars.aspx" class="btn cta-button-black px-5 py-2 fw-bold" style="color: #000000 !important; background-color: white !important; -webkit-text-fill-color: initial !important;">לצפייה ברכבים</a>
    </div>
</section>

</asp:Content>

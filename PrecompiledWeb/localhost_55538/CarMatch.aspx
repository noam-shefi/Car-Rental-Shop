<%@ page title="התאמת רכב לטיול" language="C#" masterpagefile="~/MasterPageVisitor.master" autoeventwireup="true" inherits="CarMatch, App_Web_vkkzr0a4" async="true" %>

<asp:Content ID="MatcherContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main class="container py-4" dir="rtl">
        <h1 class="text-white mb-2">התאמת רכב חכמה לטיול</h1>
        <p class="text-secondary mb-4">תארו את הטיול בטקסט או בקול וקבלו המלצה מותאמת מתוך רכבים זמינים במלאי.</p>

        <div class="card bg-dark text-white border-secondary p-4 shadow-sm">
            <label for="txtTrip" class="form-label">פרטי הטיול</label>
            <asp:TextBox ID="txtTrip" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6" ClientIDMode="Static" placeholder="לדוגמה: משפחה של חמישה נוסעים לצפון, עם הרבה ציוד ומזוודות." />

            <div class="voice-box mt-3 p-3 rounded border border-secondary-subtle">
                <div class="d-flex flex-wrap gap-2 align-items-center">
                    <button id="btnRecord" type="button" class="btn btn-outline-light">
                        <i class="bi bi-mic-fill"></i>
                        <span id="recordButtonLabel">התחלת הקלטה</span>
                    </button>
                    <span id="speechStatus" class="small text-secondary">לחצו להקלטה קולית בעברית</span>
                    <span id="recordingDot" class="recording-dot" style="display:none" aria-hidden="true"></span>
                </div>
                <div id="waveform" class="waveform mt-3" style="display:none" aria-hidden="true">
                    <span class="wave"></span><span class="wave"></span><span class="wave"></span><span class="wave"></span>
                    <span class="wave"></span><span class="wave"></span><span class="wave"></span><span class="wave"></span>
                    <span class="wave"></span><span class="wave"></span><span class="wave"></span><span class="wave"></span>
                    <span class="wave"></span><span class="wave"></span><span class="wave"></span><span class="wave"></span>
                </div>
            </div>

            <div class="d-flex flex-wrap gap-2 mt-3">
                <asp:Button ID="btnRecommend" runat="server" Text="המליצו לי על רכב" CssClass="btn btn-primary" OnClick="btnRecommend_Click" OnClientClick="return beginRecommendationRequest();" />
                <span id="recommendLoading" class="text-info small align-self-center" style="display:none">
                    <span class="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true"></span>
                    מעבד בקשה ומייצר המלצה...
                </span>
            </div>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="d-block text-warning mt-3" />

        <asp:Panel ID="pnlRecommendation" runat="server" Visible="false" CssClass="card bg-dark text-white border-info p-4 mt-4">
            <h2 class="h4 mb-2">ההמלצה שלנו: <asp:Literal ID="litCar" runat="server" /></h2>
            <p class="mb-2"><asp:Literal ID="litReason" runat="server" /></p>
            <p class="small text-info mb-2"><asp:Literal ID="litSpecs" runat="server" /></p>
            <p class="small text-warning mb-3"><asp:Literal ID="litMissingInfo" runat="server" /></p>
            <asp:HyperLink ID="lnkCar" runat="server" CssClass="btn btn-outline-info align-self-start">לצפייה בפרטי הרכב</asp:HyperLink>
        </asp:Panel>
    </main>

    <style>
        .voice-box {
            background: rgba(255,255,255,0.03);
        }

        .recording-dot {
            width: 10px;
            height: 10px;
            border-radius: 50%;
            background: #ef4444;
            box-shadow: 0 0 0 0 rgba(239,68,68,0.6);
            animation: pulseDot 1.2s infinite;
        }

        @keyframes pulseDot {
            0% { box-shadow: 0 0 0 0 rgba(239,68,68,0.6); }
            70% { box-shadow: 0 0 0 10px rgba(239,68,68,0); }
            100% { box-shadow: 0 0 0 0 rgba(239,68,68,0); }
        }

        .waveform {
            height: 54px;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 4px;
        }

        .wave {
            width: 4px;
            height: 8px;
            border-radius: 10px;
            background: #64748b;
            transition: height .08s ease, background-color .12s ease;
        }
    </style>

    <script>
        // @ts-nocheck
        (function () {
            var state = {
                mediaRecorder: null,
                isRecording: false,
                chunks: [],
                fileExt: 'webm',
                stream: null,
                audioContext: null,
                analyser: null,
                source: null,
                dataArray: null,
                animationId: null
            };

            var btn = document.getElementById('btnRecord');
            var btnLabel = document.getElementById('recordButtonLabel');
            var status = document.getElementById('speechStatus');
            var waveform = document.getElementById('waveform');
            var recordingDot = document.getElementById('recordingDot');
            var tripText = document.getElementById('txtTrip');

            if (!btn || !status || !waveform || !tripText) return;

            btn.addEventListener('click', function () {
                if (state.isRecording) stopRecording();
                else startRecording();
            });

            async function startRecording() {
                if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia || typeof MediaRecorder === 'undefined') {
                    status.textContent = 'הדפדפן אינו תומך בהקלטה. נסו דפדפן עדכני.';
                    return;
                }

                try {
                    state.stream = await navigator.mediaDevices.getUserMedia({ audio: true });
                } catch (err) {
                    status.textContent = 'אין הרשאה למיקרופון או שהגישה נחסמה.';
                    return;
                }

                try {
                    var AudioContextCtor = window.AudioContext || window.webkitAudioContext;
                    if (!AudioContextCtor) throw new Error('AudioContext not supported');

                    state.audioContext = new AudioContextCtor();
                    if (state.audioContext.state === 'suspended') await state.audioContext.resume();

                    state.analyser = state.audioContext.createAnalyser();
                    state.analyser.fftSize = 256;
                    state.analyser.smoothingTimeConstant = 0.8;
                    state.source = state.audioContext.createMediaStreamSource(state.stream);
                    state.source.connect(state.analyser);
                    state.dataArray = new Uint8Array(state.analyser.fftSize);

                    var options = {};
                    if (MediaRecorder.isTypeSupported('audio/webm')) {
                        options.mimeType = 'audio/webm';
                        state.fileExt = 'webm';
                    } else if (MediaRecorder.isTypeSupported('audio/mp4')) {
                        options.mimeType = 'audio/mp4';
                        state.fileExt = 'mp4';
                    } else {
                        cleanupAudioResources();
                        status.textContent = 'סוג הקלטה נתמך לא זמין בדפדפן זה.';
                        return;
                    }

                    state.mediaRecorder = new MediaRecorder(state.stream, options);
                    state.chunks = [];

                    state.mediaRecorder.ondataavailable = function (event) {
                        if (event.data && event.data.size > 0) state.chunks.push(event.data);
                    };

                    state.mediaRecorder.onstop = async function () {
                        status.textContent = 'שולח הקלטה לתמלול...';
                        var blob = new Blob(state.chunks, { type: options.mimeType });
                        if (!blob || blob.size < 1000) {
                            status.textContent = 'לא נקלט שמע ברור. נסו שוב.';
                            return;
                        }

                        try {
                            var base64Audio = await blobToBase64(blob);
                            var transcription = await transcribe(base64Audio, state.fileExt);
                            if (transcription.success) {
                                tripText.value = transcription.text;
                                status.textContent = 'התמלול הושלם. ניתן לערוך את הטקסט לפני המלצה.';
                            } else {
                                status.textContent = transcription.message || 'התמלול נכשל.';
                            }
                        } catch (error) {
                            status.textContent = 'שגיאת רשת או תמלול. נסו שוב.';
                        }
                    };

                    state.isRecording = true;
                    state.mediaRecorder.start(250);
                    btn.classList.remove('btn-outline-light');
                    btn.classList.add('btn-danger');
                    btnLabel.textContent = 'עצירת הקלטה';
                    recordingDot.style.display = 'inline-block';
                    waveform.style.display = 'flex';
                    status.textContent = 'מקליט... לחצו שוב לסיום.';
                    animateWaveform();
                } catch (error) {
                    cleanupAudioResources();
                    status.textContent = 'לא ניתן להתחיל הקלטה בדפדפן זה.';
                }
            }

            async function stopRecording() {
                state.isRecording = false;

                if (state.animationId) {
                    cancelAnimationFrame(state.animationId);
                    state.animationId = null;
                }

                if (state.mediaRecorder && state.mediaRecorder.state !== 'inactive') {
                    state.mediaRecorder.stop();
                }

                cleanupAudioResources();

                btn.classList.remove('btn-danger');
                btn.classList.add('btn-outline-light');
                btnLabel.textContent = 'התחלת הקלטה';
                recordingDot.style.display = 'none';
                waveform.style.display = 'none';
            }

            function cleanupAudioResources() {
                if (state.stream) {
                    state.stream.getTracks().forEach(function (track) { track.stop(); });
                    state.stream = null;
                }

                if (state.source) {
                    try { state.source.disconnect(); } catch (e) { }
                    state.source = null;
                }

                if (state.audioContext && state.audioContext.state !== 'closed') {
                    state.audioContext.close().catch(function () { });
                }

                state.audioContext = null;
                state.analyser = null;
                state.dataArray = null;
            }

            function animateWaveform() {
                if (!state.isRecording || !state.analyser || !state.dataArray) return;

                state.analyser.getByteTimeDomainData(state.dataArray);

                var sum = 0;
                for (var i = 0; i < state.dataArray.length; i++) {
                    var sample = (state.dataArray[i] - 128) / 128;
                    sum += sample * sample;
                }

                var volume = Math.min(1, Math.sqrt(sum / state.dataArray.length) * 6);
                var bars = waveform.querySelectorAll('.wave');
                bars.forEach(function (bar, index) {
                    var center = Math.abs(index - ((bars.length - 1) / 2));
                    var falloff = Math.max(0.28, 1 - (center / (bars.length / 2)));
                    var height = 8 + (volume * 42 * falloff * (0.7 + Math.random() * 0.3));
                    bar.style.height = height + 'px';

                    if (volume > 0.7) bar.style.backgroundColor = '#ef4444';
                    else if (volume > 0.4) bar.style.backgroundColor = '#eab308';
                    else bar.style.backgroundColor = '#64748b';
                });

                state.animationId = requestAnimationFrame(animateWaveform);
            }

            function blobToBase64(blob) {
                return new Promise(function (resolve, reject) {
                    var reader = new FileReader();
                    reader.onloadend = function () {
                        if (!reader.result) {
                            reject(new Error('No audio data'));
                            return;
                        }
                        resolve(reader.result.toString().split(',')[1]);
                    };
                    reader.onerror = reject;
                    reader.readAsDataURL(blob);
                });
            }

            async function transcribe(base64Audio, fileExt) {
                var response = await fetch('CarMatch.aspx/TranscribeAudio', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json; charset=utf-8' },
                    body: JSON.stringify({ base64Audio: base64Audio, fileExt: fileExt })
                });

                var payload = await response.json();
                if (!payload || !payload.d) {
                    return { success: false, text: '', message: 'התקבלה תשובה לא תקינה מהשרת.' };
                }

                return {
                    success: !!payload.d.Success,
                    text: payload.d.Text || '',
                    message: payload.d.Message || ''
                };
            }

            window.beginRecommendationRequest = function () {
                var text = (tripText.value || '').trim();
                if (!text) {
                    status.textContent = 'נא להזין או להקליט תיאור נסיעה לפני בקשת המלצה.';
                    return false;
                }

                var loading = document.getElementById('recommendLoading');
                var recommendButton = document.getElementById('<%= btnRecommend.ClientID %>');
                if (loading) loading.style.display = 'inline-flex';

                // Important: disabling immediately inside OnClientClick can cancel WebForms postback in some browsers.
                if (recommendButton) {
                    window.setTimeout(function () {
                        recommendButton.disabled = true;
                    }, 0);
                }

                return true;
            };

            window.addEventListener('beforeunload', function () {
                state.isRecording = false;
                cleanupAudioResources();
            });
        }());
    </script>
</asp:Content>

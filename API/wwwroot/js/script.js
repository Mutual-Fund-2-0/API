// Modern approach - NO DOMContentLoaded needed
(() => {
    const forecastBtn = document.getElementById('forecast');
    const resultDiv = document.getElementById('result');
    forecastBtn.addEventListener('click', async () => {
        try {
            const response = await fetch('/weatherforecast');
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}: ${response.statusText}`);
            }
            const weatherData = await response.json();
            let html = '<h3>Weather Data:</h3><ul>';
            weatherData.forEach(item => {
                html += `<li><strong>${item.date}:</strong> ${item.temperatureC}°C ${item.temperatureF}°F - ${item.summary}</li>`;
            });
            html += '</ul>';
            resultDiv.innerHTML = html;
        } catch (error) {
            resultDiv.innerHTML = `<p style="color: red;">Error: ${error.message}</p>`;
        } finally {
            forecastBtn.disabled = false;
        }
    });
})();

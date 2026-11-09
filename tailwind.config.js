/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class',
  content: [
    './src/**/*.{razor,html,cshtml,cs}',
    './src/Net11Showcase.Web/Net11Showcase.Web/Components/**/*.{razor,html}',
    './src/Net11Showcase.Components/**/*.{razor,html,cs}'
  ],
  theme: {
    extend: {
      colors: {
        slate: {
          750: '#233044',
          850: '#172033',
          950: '#020617'
        },
        brand: {
          50: '#eef2ff',
          100: '#e0e7ff',
          200: '#c7d2fe',
          300: '#a5b4fc',
          400: '#818cf8',
          500: '#6366f1',
          600: '#4f46e5',
          700: '#4338ca',
          800: '#3730a3'
        }
      }
    }
  },
  plugins: []
};

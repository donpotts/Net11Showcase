/** Builds only the utilities used by the Agent Ops module. Preflight is off: the shell's stylesheet already resets. */
module.exports = {
  darkMode: 'class',
  corePlugins: { preflight: false },
  content: [
    './src/Net11Showcase.Components/AgentOps/**/*.{razor,cs}',
    './src/Net11Showcase.Web/Net11Showcase.Web/Components/Pages/AgentOps.razor'
  ],
  theme: {
    extend: {
      colors: {
        slate: { 750: '#233044', 850: '#172033' },
        // The components use a `brand` palette; map it to indigo for the showcase.
        brand: {
          50: '#eef2ff', 100: '#e0e7ff', 200: '#c7d2fe', 300: '#a5b4fc', 400: '#818cf8',
          500: '#6366f1', 600: '#4f46e5', 700: '#4338ca', 800: '#3730a3'
        }
      }
    }
  }
};

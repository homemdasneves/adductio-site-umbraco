/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["../Views/**/*.cshtml"],
  theme: {
    extend: {
      colors: {
        adductioOrange: "#F15A31",
        adductioBlue: "#2D2B70",
        adductioPaleBlue: "#E1EAF7",
        adductioSoftGrey: "#F1F2F6",
        adductioMiddleGrey: "#6E707F",
      }
    },
  },
  plugins: []
}

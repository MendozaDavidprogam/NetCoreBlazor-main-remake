window.appLang = {
    es: {
        panel: "Panel Culinario",
        home: "Home",
        perfil: "Perfil",
        recetas: "Recetas",
        ingredientes: "Ingredientes",
        pasos: "Pasos",
        tipoIngredientes: "Tipo Ingredientes",
        tipoRecetas: "Tipo Recetas",
        unidades: "Unidades De Medición",
        usuarios: "Usuarios Registrados",
        cerrarSesion: "Cerrar sesión",
        modoOscuro: "Modo Oscuro",
        modoClaro: "Modo Claro"
    },
    en: {
        panel: "Culinary Panel",
        home: "Home",
        perfil: "Profile",
        recetas: "Recipes",
        ingredientes: "Ingredients",
        pasos: "Steps",
        tipoIngredientes: "Ingredient Types",
        tipoRecetas: "Recipe Types",
        unidades: "Units",
        usuarios: "Registered Users",
        cerrarSesion: "Logout",
        modoOscuro: "Dark Mode",
        modoClaro: "Light Mode"
    }
};

window.setLanguage = function (lang) {
    document.documentElement.setAttribute("lang", lang);
    localStorage.setItem("app_language", lang);
};

window.translate = function (key) {
    const lang = localStorage.getItem("app_language") || "es";
    return window.appLang[lang]?.[key] || key;
};

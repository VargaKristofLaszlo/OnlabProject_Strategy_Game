function HoverBehaviour()
{
    var woodCapacity = document.getElementById("wood-capacity");
    var woodProduction = document.getElementById("wood-production");
    var stoneCapacity = document.getElementById("stone-capacity");
    var stoneProduction = document.getElementById("stone-production");
    var silverCapacity = document.getElementById("silver-capacity");
    var silverProduction = document.getElementById("silver-production");
    var maxPopulation = document.getElementById("max-population");
    var spearmanNameTooltip = document.getElementById("tooltip-spearman-name");
    var swordsmanNameTooltip = document.getElementById("tooltip-swordsman-name");
    var axeFighterNameTooltip = document.getElementById("tooltip-axe-fighter-name");
    var archerNameTooltip = document.getElementById("tooltip-archer-name");
    var lightCavalryNameTooltip = document.getElementById("tooltip-light-cavalry-name");
    var heavyCavalryNameTooltip = document.getElementById("tooltip-heavy-cavalry-name");
    var mountedArcherNameTooltip = document.getElementById("tooltip-mounted-archer-name");
   


    window.onmousemove = function (e) {
        var x = e.clientX,
            y = e.clientY;       

        spearmanNameTooltip.style.top = y + 20 + "px";
        spearmanNameTooltip.style.left = x + 20 + "px";

        swordsmanNameTooltip.style.top = y + 20 + "px";
        swordsmanNameTooltip.style.left = x + 20 + "px";

        axeFighterNameTooltip.style.top = y + 20 + "px";
        axeFighterNameTooltip.style.left = x + 20 + "px";

        archerNameTooltip.style.top = y + 20 + "px";
        archerNameTooltip.style.left = x + 20 + "px";

        lightCavalryNameTooltip.style.top = y + 20 + "px";
        lightCavalryNameTooltip.style.left = x + 20 + "px";

        heavyCavalryNameTooltip.style.top = y + 20 + "px";
        heavyCavalryNameTooltip.style.left = x + 20 + "px";

        mountedArcherNameTooltip.style.top = y + 20 + "px";
        mountedArcherNameTooltip.style.left = x + 20 + "px";

        woodCapacity.style.top = y + 20 + "px";
        woodCapacity.style.left = x + 20 + "px";

        woodProduction.style.top = y + 45 + "px";
        woodProduction.style.left = x + 20 + "px";

        stoneCapacity.style.top = y + 20 + "px";
        stoneCapacity.style.left = x + 20 + "px";

        stoneProduction.style.top = y + 45 + "px";
        stoneProduction.style.left = x + 20 + "px";

        silverCapacity.style.top = y + 20 + "px";
        silverCapacity.style.left = x + 20 + "px";

        silverProduction.style.top = y + 45 + "px";
        silverProduction.style.left = x + 20 + "px";

        maxPopulation.style.top = y + 20 + "px";
        maxPopulation.style.left = x + 20 + "px";
    };
}

function unitsOfCityViewHoverBehaviour() {
    var spearmanNameTooltip = document.getElementById("tooltip-spearman-name");
    var swordsmanNameTooltip = document.getElementById("tooltip-swordsman-name");
    var axeFighterNameTooltip = document.getElementById("tooltip-axe-fighter-name");
    var archerNameTooltip = document.getElementById("tooltip-archer-name");
    var lightCavalryNameTooltip = document.getElementById("tooltip-light-cavalry-name");
    var heavyCavalryNameTooltip = document.getElementById("tooltip-heavy-cavalry-name");
    var mountedArcherNameTooltip = document.getElementById("tooltip-mounted-archer-name");

    window.onmousemove = function (e) {
        var x = e.clientX,
            y = e.clientY;


        spearmanNameTooltip.style.top = y + 20 + "px";
        spearmanNameTooltip.style.left = x + 20 + "px";

        swordsmanNameTooltip.style.top = y + 20 + "px";
        swordsmanNameTooltip.style.left = x + 20 + "px";

        axeFighterNameTooltip.style.top = y + 20 + "px";
        axeFighterNameTooltip.style.left = x + 20 + "px";

        archerNameTooltip.style.top = y + 20 + "px";
        archerNameTooltip.style.left = x + 20 + "px";

        lightCavalryNameTooltip.style.top = y + 20 + "px";
        lightCavalryNameTooltip.style.left = x + 20 + "px";

        heavyCavalryNameTooltip.style.top = y + 20 + "px";
        heavyCavalryNameTooltip.style.left = x + 20 + "px";

        mountedArcherNameTooltip.style.top = y + 20 + "px";
        mountedArcherNameTooltip.style.left = x + 20 + "px";
    };
}
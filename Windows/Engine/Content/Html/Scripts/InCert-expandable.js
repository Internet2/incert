(function() {
    function adjustExpandable() {
        var expandable = $(".expandable");
        if (expandable.length !==1) {
            return;
        }

        var siblingsHeight = 0;
        expandable.siblings().each(function () {
            siblingsHeight = siblingsHeight + $(this).outerHeight(true);
        });

        $(window).unbind("resize");

        var marginheight = expandable.outerHeight(true) - expandable.outerHeight();

        var expandableHeight = $(window).height() - siblingsHeight - marginheight;
        expandable.css("max-height", expandableHeight);
        $(window).resize(adjustExpandable);

    }

    $(document).ready(function () {
        adjustExpandable();
    });
})();
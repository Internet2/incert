(function() {
    function adjustExpandable() {
        var expandable = $(".expandable");
        if (expandable.length === 0) {
            return;
        }

        var siblingsHeight = 0;
        expandable.siblings().each(function () {
            siblingsHeight = siblingsHeight + this.clientHeight;
        });

        $(window).unbind("resize");
        var expandableHeight = $(window).height() - siblingsHeight;
        expandable.css("max-height", expandableHeight);
        $(window).resize(adjustExpandable);

    }

    $(document).ready(function () {
        adjustExpandable();
    });
})();
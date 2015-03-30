(function () {
    var runningItem = false;

    function initializeTitle() {
        $.each($("[data-advanced-menu-title]"), function () {
            $(this).text($(this).data("advanced-menu-title"));
        });
    };

    function initializeDescription() {
        $.each($("[data-advanced-menu-description]"), function () {
            $(this).text($(this).data("advanced-menu-description"));
        });
    }

    function setTitle(value) {
        $("[data-advanced-menu-title]").text(engine.resolveValue(value));
    }

    function setDescription(value) {
        $("[data-advanced-menu-description]").text(engine.resolveValue(value));
    }

    function addGroupElements() {
        var target = $("[data-advanced-menu-items-container]");

        var items = deserializeItems();
        var groups = groupItems(items);
        for (var propertyName in groups) {
            if (groups.hasOwnProperty(propertyName)) {
                appendGroupTitleElement(propertyName, target);

                addGroupItemsElement(groups[propertyName], target);
            }
        }
        target.append(target);
    }

    function deserializeItems() {
        return $.parseJSON(engine.getAdvancedMenuItems());
    }

    function appendGroupTitleElement(name, table) {
        var element = $("<div class='row'><div class='col-xs-12'><h4 class='menu-group-title'>" + name + "</h4></div></div>");
        table.append(element);
    }

   

    function addGroupItemsElement(items, table) {
        var count = 0;

        var rowElements = [];
        var currentRow;
        $.each(items, function () {
            if (count === 0) {
                rowElements.push($("<div class='row'></div>"));
                currentRow = rowElements[rowElements.length - 1];
            }

            var cellElement = $("<div class='col-xs-2 menu-item'></div");
            cellElement.append(createItemButtonElement(this));
            cellElement.append(createItemTitleElement(this));
            cellElement.data("item", this);
            cellElement.hover(null, hoverOutHandler);

            currentRow.append(cellElement);

            count++;
            if (count > 4) {
                count = 0;
            }
        });

        table.append(rowElements);
    }

    function hoverInHandler(event) {
        if (runningItem) {
            return;
        }

        var container = $(event.target).closest(".menu-item");
        focusItems(container);

        var item = container.data("item");
        if (!item) return;

        setTitle(item.title);
        setDescription(item.description);
        showCogIcon(container);
    }

    function showCogIcon(container) {
        var target = $(container).find(".itemIcon");
        target.removeClass("fa-spin");
        target.show();
    }

    function spinCogIcon(container) {
        var target = $(container).find(".itemIcon");
        target.addClass("fa-spin");
        target.show();
    }

    function hoverOutHandler(event) {
        if (runningItem) {
            return;
        }

        var container = $(event.target).closest(".menu-item");
        unfocusItems(container);

        initializeTitle();
        initializeDescription();
        hideIcons();
    }

    function createItemButtonElement(item) {
        var buttonElement = $("<button class='number-circle'></button>");
        var cogElement = $("<i class='fa fa-fw fa-cog itemIcon'></i>");
        cogElement.hide();
        buttonElement.append(cogElement);
        buttonElement.append($("<span class='itemText'>" + item.buttonText + "</span>"));
        buttonElement.click(itemClickHandler);
        buttonElement.hover(hoverInHandler);
        return buttonElement;
    }

    function createItemTitleElement(item) {
        var titleElement = $("<p></p>");
        var titleAnchor = $("<a href='' class='menu-item-title'></a>");
        titleAnchor.text(engine.resolveValue(item.title));
        titleAnchor.data("item", item);
        titleAnchor.click(itemClickHandler);
        titleElement.append(titleAnchor);
        titleElement.hover(hoverInHandler);
        return titleElement;
    }

    function itemClickHandler(event) {
        if (runningItem) {
            return;
        }

        event.preventDefault();

        var container = $(event.target).closest(".menu-item");
        if (!container) return;

        var item = container.data("item");
        if (!item) return;

        setTitle(item.workingTitle);
        setDescription(item.workingDescription);

        runningItem = true;
        disableAllControls();
        spinCogIcon(container);
        setTimeout(function () { engine.runTaskBranch(item.branch); }, 1500);

    }
    
    function hideIcons() {
        $(".itemIcon").hide();
        $(".itemText").show();
    }

    function disableAllControls() {
        $(":input").attr("disabled", true);
        $(":button").attr("disabled", true);
        $(".menu-item-title, .menu-group-title, .number-circle").addClass("disabled");
    }

    function enableAllControls() {
        $(":input").attr("disabled", false);
        $(":button").attr("disabled", false);
        $(".menu-item-title, .menu-group-title, .number-circle").removeClass("disabled");
    }

    function focusItems(container) {
        if (!container) return;

        var items = $(container).find(".number-circle,.menu-item-title");
        items.addClass("focused");
    }

    function unfocusItems(container) {
        if (!container) {
            return;
        }

        var items = $(container).find(".number-circle,.menu-item-title");
        items.removeClass("focused");
    }

    function groupItems(items) {
        var result = {};

        if (!items) {
            return result;
        }

        $.each(items, function () {
            if (!groupValid(this.group)) {
                return true; // continue to next item
            }

            if (!result.hasOwnProperty(this.group)) {
                result[this.group] = [];
            }

            result[this.group].push(this);
            return true;
        });
        return result;
    }

    function resetHover() {
        
        $(".menu-item-title, .number-circle").removeClass("focused");
        var current = $(".menu-item-title:hover, .number-circle:hover");
        if (current.length === 0) {
            return;
        }
        var container = $(current[0]).closest(".menu-item");
        console.log(container);
        focusItems(container);
        showCogIcon(container);
    }

    function groupValid(value) {
        return value && value.length > 0;
    }

    $(document).ready(function () {
        initializeTitle();
        initializeDescription();
        hideIcons();
        addGroupElements();
    });

    $(document).on("engine_advanced_menu_branch_finish", function (event, data) {
        enableAllControls();
        initializeDescription();
        initializeTitle();
        hideIcons();
        resetHover();
        runningItem = false;
       
    });

})();



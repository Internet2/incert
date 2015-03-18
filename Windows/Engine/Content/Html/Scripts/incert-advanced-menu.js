(function() {
    function initializeTitle() {
        $.each($("[data-advanced-menu-title]"), function() {
            $(this).text($(this).data("advanced-menu-title"));
        });
    };

    function initializeDescription() {
        $.each($("[data-advanced-menu-description]"), function () {
            $(this).text($(this).data("advanced-menu-description"));
        });
    }

    function addGroupElements() {
        var target = $("[data-advanced-menu-items-container]");
        console.log(target);
        var groups = groupItems(engine.getAdvancedMenuItems());
        for (var propertyName in groups) {
            if (groups.hasOwnProperty(propertyName)) {
                var titleElement = createGroupTitleElement(propertyName);
                var itemsElement = createGroupItemsElement(groups[propertyName]);
                target.append(titleElement);
                target.append(itemsElement);
            }
        }
    }

    function createGroupTitleElement(name) {
        var result = "<div class='row'><div class='col-sm-12'><h4>" + name + "</h4></div></div>";
        return $(result);
    }

    function createGroupItemsElement(items) {
        var rowElement = $("<div class='row'></div>");
        $.each(items, function() {
            var divElement = $("<div class='col-sm-2 menu-item'></div");
            divElement.append(createItemButtonElement(this));
            divElement.append(createItemTitleElement(this));
            divElement.data("item", this);
            divElement.hover(null, hoverOutHandler);
            rowElement.append(divElement);
        });
       
        return rowElement;
    }

    function hoverInHandler() {
        var container = $(event.target).closest(".menu-item");
        focusItems(container);

        var item = container.data("item");
        if (!item) return;


        $("[data-advanced-menu-title]").text(engine.resolveValue(item.title));
        $("[data-advanced-menu-description]").text(engine.resolveValue(item.description));

    }

    function hoverOutHandler() {
        var container = $(event.target).closest(".menu-item");
        unfocusItems(container);

        initializeTitle();
        initializeDescription();
    }

    function createItemButtonElement(item) {
        var buttonElement = $("<button class='number-circle'></button>");
        buttonElement.text(item.buttonText);
        buttonElement.click(itemClickHandler);
        buttonElement.hover(hoverInHandler);
        return buttonElement;
    }

    function createItemTitleElement(item) {
        var titleElement = $("<p></p>");
        var titleAnchor = $("<a href='' class='menu-item-title'></a>");
        titleAnchor.text(engine.resolveValue(item.title));
        titleAnchor.data("item", item);
        titleAnchor.attr("id", item.key);
        titleAnchor.click(itemClickHandler);
        titleElement.append(titleAnchor);
        titleElement.hover(hoverInHandler);
        return titleElement;
    }
    
   /* function focusOutHandler(event) {
        var container = $(event.target).closest(".menu-item");
        
        unfocusItems(container);
      
        if ($(event.relatedTarget).is("button.number-circle") || $(event.relatedTarget).is("a.menu-item-title")) {
            return;
        }
        
        initializeTitle();
        initializeDescription();
    }*/

    function itemClickHandler(event) {
        console.log("click");
     /*   event.preventDefault();
        event.stopPropagation();

        var container = $(event.target).closest(".menu-item");
        if (!container) return;

        var item = container.data("item");
        if (!item) return;

        focusItems(container);
      
        $("[data-advanced-menu-title]").text(engine.resolveValue(item.title));
        $("[data-advanced-menu-description]").text(engine.resolveValue(item.description));*/
    }
    
    function setStartIcon(target) {
        var items = $(target).closest(".menu-item").find(".number-circle");
        items.find("span").hide();
        items.find("i").show();
    }

    function removeStartIcon(target) {
        var items = $(target).closest(".menu-item").find(".number-circle");
        items.find("span").show();
        items.find("i").hide();
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

    function groupValid(value) {
        return value && value.length > 0;
    }

    $(document).ready(function () {
        console.log("initializing advanced menu");
        initializeTitle();
        initializeDescription();
        addGroupElements();
    });

})();

//initInCertAdvancedMenuSupport();

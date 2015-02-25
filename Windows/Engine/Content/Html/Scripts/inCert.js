function init() {
    if (document.engineScriptVersion) {
        if (document.engineScriptVersion >= 1.0) {
            return;
        }
    }

    document.engineScriptVersion = 1.0;

    function disableAllControls() {
        $(":input").attr("disabled", true);
    }

    function enableAllControls() {
        $(":input").attr("disabled", false);
    }

    function globalInitialize() {
        console.log("global initialize called");
        resolveValues();
        getInputValues();
        getCheckBoxValues();
        addButtonEvents();
        addChangeEvents();
        configureInitialVisibility();
        configureInitialEnabledState();
        configureValidation();
    }

    function resolveValues() {
        $("[data-resolve]").each(function () {
            $(this).text(engine.resolveValue($(this).data("resolve")));
        });
    }

    function getInputValues() {
        $("input:text[data-setting], input:password[data-setting]").each(function () {
            var setting = $(this).data("setting");
            $(this).val(engine.getValue(setting));
        });
    }

    function getCheckBoxValues() {
        $("input:checkbox[data-setting]").each(function () {
            var setting = $(this).data("setting");
            var value = engine.getValue(setting).toLowerCase() === "true";
            $(this).prop("checked", value);
        });
    }

    function addButtonEvents() {
        $("[data-next-button]").click(returnNextClickHandler);
        $("[data-back-button]").click(returnBackClickHandler);
        $("[data-close-button]").click(returnCloseClickHandler);
        $("[data-help-button]").click(showHelpTopicClickHandler);
        $("[data-error-button]").click(returnErrorClickHandler);
        $("[data-advanced-menu-button]").click(showAdvancedMenuButtonClickHandler);
    }

    function addChangeEvents() {
        $("input:text[data-setting], input:password[data-setting]").change(inputValueChangeHandler);
        $("input:checkbox[data-setting]").change(checkBoxValueChangeHandler);
    }

    function configureInitialVisibility() {
        $("[data-show]").hide();
        $("[data-hide]").show();
    }

    function configureInitialEnabledState() {


        $("[data-enable]").attr("disabled", true);
        $("[data-disable]").attr("disabled", false);
    }

    function configureValidation() {
        if (!$("[data-validate-form]").length) {
            return;
        }

        $("form").validate({
            showErrors: getValidationErrorHandler()
        });
    }

    function validateForm(target) {
        if (!$(target).filter("[data-validate-form]").length) {
            return true;
        }

        return ($("form").valid());
    }

    function getValidationErrorHandler() {
        return bootstrapPresent()
            ? popoverValidationHandler
            : undefined;
    }

    function bootstrapPresent() {
        return $.fn.popover;
    }

    function popoverValidationHandler(errorMap, errorList) {
        $.each(this.successList, function (index, value) {
            return $(value).popover("destroy");
        });
        return $.each(errorList, function (index, value) {
            console.log(value.message);
            var result = $(value.element).popover({
                trigger: "manual",
                placement: "top",
                content: value.message
            });
            
            var resulta = $(value.element).popover("show");
            return resulta;
        });
    }

    function returnNextClickHandler(event) {
        if (!validateForm(event.target)) {
            return;
        }

        engine.returnNext();
    }

    function showAdvancedMenuButtonClickHandler(event) {
        var group = $(event.target).data("advanced-menu-group");
        disableAllControls();
        engine.showAdvancedMenu(group);
        enableAllControls();
    }

    function returnBackClickHandler() {
        if (!validateForm(event.target)) {
            return;
        }

        engine.returnBack();
    }

    function returnCloseClickHandler() {
        if (!validateForm(event.target)) {
            return;
        }

        engine.returnClose();
    }

    function inputValueChangeHandler(event) {
        var setting = $(event.target).data("setting");
        engine.setValue(setting, $(event.target).val());
    }

    function checkBoxValueChangeHandler(event) {
        var setting = $(event.target).data("setting");
        var value = !($(this).prop("checked")) ? "False" : "True";
        engine.setValue(setting, value);
    }

    function showHelpTopicClickHandler(event) {
        var topic = $(event.target).data("help-topic");
        engine.showHelpTopic(topic);
    }

    function returnErrorClickHandler(event) {
        var errorType = $(event.target).data("error-type");
        engine.returnError(errorType);
    }

    function getMessageFromData(data) {
        if (!data) { return undefined; }

        if (!data.message || !data.message.length) { return undefined; }

        return data.message;
    }

    function getTaskIdFromData(data) {
        if (!data) { return undefined; }

        if (!data.id || !data.id.length) { return undefined; }

        return data.id;
    }

    function configureForInBrowserTesting() {
        if (typeof engine !== "undefined") {
            return;
        }

        console.log("InCert engine not detected. Adding mock methods to simulate running inside InCert engine.");

        engine = {};

        engine.getValue = function (property) {
            return property + " value";
        }

        engine.resolveValue = function (value) {
            return value + " value";
        }

        engine.setValue = function (property, value) {
            console.log("setting " + property + " to " + value);
        }

        engine.returnNext = function () {
            console.log("returning next value");
        }

        engine.returnBack = function () {
            console.log("returning back value");
        }

        engine.returnClose = function () {
            console.log("returning close value");
        }

        engine.showHelpTopic = function (topic) {
            console.log("showing help topic " + topic);
        }

        engine.returnError = function (errorType) {
            console.log("returning error result (" + errorType + ")");
        }

        engine.showAdvancedMenu = function (group) {
            console.log("showing advanced menu (group = '" + group + "')");
        }


    }

    document.raiseEngineEvent = function (type, data) {
        $(document).trigger(type, data);
    }

    function getEventSelector(taskId) {
        return taskId
            ? ":not([data-task-id]),[data-task-id='" + taskId + "']"
            : ":not([data-task-id])";
    }

    function processTaskEvents(targets, data) {
        var taskId = getTaskIdFromData(data);

        var selector = getEventSelector(taskId);
        var affected = targets.filter(selector);
        if (!affected.length) { return; }

        affected.filter("[data-disable]").attr("disabled", true);
        affected.filter("[data-enable]").attr("disabled", false);
        affected.filter("[data-show]").show();
        affected.filter("[data-hide]").hide();

        var message = getMessageFromData(data);
        if (message) {
            affected.filter("[data-task-message]").text(message);
        }
    }

    function processGlobalTaskMessages(data) {
        var message = getMessageFromData(data);
        if (!message) {
            return;
        }

        var taskId = getTaskIdFromData(data);
        var selector = getEventSelector(taskId);
        var targets = $("[data-task-message]:not([data-on-task-start]):not(data-on-task-finish)")
            .filter(selector);

        console.log(targets);
        targets.text(message);
    }

    configureForInBrowserTesting();

    ellipsis = {
        'value': ['', '.', '..', '...'],
        'count': 0,
        'run': false,
        'timer': null,
        'element': '.ellipsis',
        'start': function () {
            var t = this;
            this.run = true;
            this.timer = setInterval(function () {
                if (t.run) {
                    $(t.element).html(t.value[t.count % t.value.length]).text();
                    t.count++;
                }
            }, 500);
        },
        'stop': function () {
            this.run = false;
            clearInterval(this.timer);
            this.count = 0;
        }
    }

    ellipsis.start();

    $(document).ready(function () {
        globalInitialize();
    });

    $(document).on("engine_task_start", function (event, data) {
        processTaskEvents($("[data-on-task-start]"), data);
        processGlobalTaskMessages(data);

    });

    $(document).on("engine_task_finish", function (event, data) {
        processTaskEvents($("[data-on-task-finish]"), data);
        processGlobalTaskMessages(data);
    });
}

init();





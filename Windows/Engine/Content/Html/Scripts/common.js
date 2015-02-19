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
        $("[data-show-on]").hide();
        $("[data-hide-on]").show();
    }

    function configureInitialEnabledState() {
        $("[data-enable-on]").attr("disabled", true);
        $("[data-disable-on]").attr("disabled", false);
    }

    function returnNextClickHandler() {
        engine.returnNext();
    }

    function showAdvancedMenuButtonClickHandler(event) {
        var group = $(event.target).data("advanced-menu-group");
        disableAllControls();
        engine.showAdvancedMenu(group);
        enableAllControls();
    }

    function returnBackClickHandler() {
        disableAllControls();
        engine.returnBack();
    }

    function returnCloseClickHandler() {
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

    function showTargetsForMessage(id, predicate) {
        var selector = getSelectorForMessage("show", id, predicate);
        $(selector).show();
    }

    function hideTargetsForMessage(id, predicate) {
        var selector = getSelectorForMessage("hide", id, predicate);
        $(selector).hide();
    }

    function enableTargetsForMessage(id, predicate) {
        var selector = getSelectorForMessage("enable", id, predicate);
        $(selector).attr("disabled", false);
    }

    function disableTargetsForMessage(id, predicate) {
        var selector = getSelectorForMessage("disable", id, predicate);
        $(selector).attr("disabled", true);
    }

    function getSelectorForMessage(method, taskId, predicate) {
        return "[data-" + method + "-on-" + predicate + "=" + taskId + "]";
    }

    function processTaskEvent(taskId, predicate) {
        showTargetsForMessage(taskId, predicate);
        hideTargetsForMessage(taskId, predicate);
        enableTargetsForMessage(taskId, predicate);
        disableTargetsForMessage(taskId, predicate);
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
        var taskId = getTaskIdFromData(data);
        processTaskEvent(taskId, "finish");
    });

    $(document).on("engine_task_finish", function (event, data) {
        var taskId = getTaskIdFromData(data);
        processTaskEvent(taskId, "finish");

    });


    $(document).on("task_started", function (event, data) {
        if (!data) { return; }

        if (!data.message || !data.message.length) { return; }

        $("[data-task-started-message]").text(data.message);
    });


    $(document).on("branch_started", function (event, data) {
        if (!data) { return; }

        if (!data.message || !data.message.length) { return; }

        $("[data-branch-started-message]").text(data.message);
    });
}

init();





var app = new Vue({
    el: '#app',
    data: function () {
        return {
            flowName: 'Default',
            flows: {},
            hasFlows: false,
            model: {}
        };
    },
    computed: {
        selectedFlow: function () {
            self = this;
            if (!self.flows) return {};
            return self.hasFlows
                ? self.flows.filter(f => f.Name === self.flowName)[0]
                : self.flows[0];
        }
    },
    methods:
    {
        loadState: function () {
            self = this;
            $.get('/Queries/', function (data) {
                self.flows = data.Flows;
                self.hasFlows = data.HasFlows;
                if (self.hasFlows) {
                    self.flowName = self.flows[0].Name;
                }
                else {
                    self.flowName = 'Default';
                }
            });
        }
    },
    mounted: function () {
        this.loadState();
    }
});
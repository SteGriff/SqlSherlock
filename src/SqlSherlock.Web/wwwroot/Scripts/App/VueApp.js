var app = new Vue({
    el: '#app',
    data: function () {
        return {
            flowName: null,
            flows: {},
            connectionName: null,
            connections: [],
            hasFlows: false,
            model: {},
            lastRunConnection: null,
            showLogo: true,
            instanceName : "Sherlock"
        };
    },
    computed: {
        selectedFlow: function () {
            self = this;
            if (!self.flows) return {};
            return self.hasFlows
                ? self.flows.filter(f => f.Name === self.flowName)[0]
                : self.flows[0];
        },
        hasConnections: function () {
            if (!this.connections) return false;
            return this.connections.length > 1;
        }
    },
    methods:
    {
        loadState: async function () {
            const self = this;
            const response = await fetch('Queries/');
            const data = await response.json();
            self.flows = data.Flows;
            self.hasFlows = data.HasFlows;
            if (!self.flowName) {
                self.flowName = self.hasFlows
                    ? self.flows[0].Name
                    : self.flowName = 'Default'
            }

            self.connections = data.Environments;
            if (!self.connectionName) {
                self.connectionName = self.connections[0].Name;
            }

            self.instanceName = data.InstanceName;
            document.title = data.InstanceName;
        },
        trackRun: function (lastRun) {
            console.log("trackRun", lastRun);
            this.lastRunConnection = lastRun;
        },
        needsRefresh: function () {
            return this.lastRunConnection !== this.connectionName;
        },
        noLogo: function () {
            this.showLogo = false;
        }
    },
    mounted: function () {
        this.loadState();
    }
});
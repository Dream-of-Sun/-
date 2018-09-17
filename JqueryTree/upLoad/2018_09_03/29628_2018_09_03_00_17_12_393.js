var anti_model_config = {
    chart: {
        container: "#gm_canvas",
        nodeAlign: "BOTTOM",
        connectors: {
            type: 'step',
            style: {
                'arrow-start': 'open-wide-long',
                "stroke-width": 2,
			}
        },
        levelSeparation: 70,
        //animateOnInit: true,
        node: {
            HTMLclass: 'gm-node',
            collapsable: true
        },
        animation: {
            nodeAnimation: "easeOutBounce",
            nodeSpeed: 700,
            connectorsAnimation: "bounce",
            connectorsSpeed: 700
        }
    },
    nodeStructure: {
        text: {
            name:"G1",
            threat: "T: Disclose",
            asset: "A: CCA",
            target: "E: store server A",
            interval:"I: ss"
        },
        HTMLclass: "light-green",
        //collapsed: true,
        children: [
            {   
                text:{
                    name:"G2",
                    threat: "T: Reach",
                    asset: "A: store server A",
                    target: "E: ",
                    interval:"I: "
                },
                connectors: {
                    type: 'straight'
                },
                HTMLclass: "light-green",
               // collapsed: true,
                children: [
                    {
                        text:{
                            name:"G4",
                            threat: "T: Reach",
                            asset: "A: store server A",
                            target: "E: local network",
                            interval:"I: "
                        },
                        HTMLclass: "light-green",
                    },
                    {
                        text:{
                            name:"G5",
                            threat: "T: Reach",
                            asset: "A: store server A",
                            target: "E: store server A",
                            interval:"I: "
                        },
                        HTMLclass: "light-green",
                    },
                    {
                        text:{
                            name:"G6",
                            threat: "T: Reach",
                            asset: "A: store server A",
                            target: "E: POS terminal",
                            interval:"I: "
                        },
                        HTMLclass: "light-green",
                    }
                ]
            },
            {
                text:{
                    name:"G3",
                    threat: "T: Access",
                    asset: "A: store server A",
                    target: "E: ",
                    interval:"I: "
                },
                HTMLclass: "light-green",
               
            }
        ]
    }
};
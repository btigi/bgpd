const GameInfo = Vue.defineCustomElement({
    template: `
    <table class="entity-table">
      <thead>
        <tr>
          <th>Name</th>
          <th>HP</th>
          <th>Position</th>
          <th>Race</th>
          <th>Class</th>
          <th>Enemy/Ally</th>
          <th>Stats</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="entity in Entities" :key="entity.Id">
          <td>{{ entity.Name2 }}</td>
          <td>{{ entity.CurrentHP }}/{{ entity.DerivedStatsTemp?.MaxHP }}</td>
          <td>({{ entity.X }}, {{ entity.Y }})</td>
          <td>{{ entity.Race }}</td>
          <td>{{ entity.Class }}</td>
          <td>{{ getEnemyAllyText(entity.EnemyAlly) }}</td>
          <td>
            STR: {{ entity.DerivedStats?.STR }}/{{ entity.DerivedStats?.STRExceptional }}
            INT: {{ entity.DerivedStats?.INT }}
            WIS: {{ entity.DerivedStats?.WIS }}
            DEX: {{ entity.DerivedStats?.DEX }}
            CON: {{ entity.DerivedStats?.CON }}
            CHA: {{ entity.DerivedStats?.CHA }}
          </td>
        </tr>
      </tbody>
    </table>
    <style>
      .entity-table {
        width: 100%;
        border-collapse: collapse;
        margin-top: 20px;
      }
      .entity-table th, .entity-table td {
        border: 1px solid #ddd;
        padding: 8px;
        text-align: left;
      }
      .entity-table th {
        background-color: #f4f4f4;
      }
      .entity-table tr:nth-child(even) {
        background-color: #f9f9f9;
      }
      .entity-table tr:hover {
        background-color: #f5f5f5;
      }
    </style>
    `,
    created() {
        this.vm = dotnetify.vue.connect("GameInfo", this);
    },
    unmounted() {
        this.vm.$destroy();
    },
    data() {
        return { 
            Entities: [],
            enemyAllyMap: {
                0: "Anyone",
                1: "Inanimate",
                2: "Regular party members",
                3: "Familiars",
                4: "Ally",
                128: "Neutral"
            }
        };
    },
    methods: {
        getEnemyAllyText(value) {
            return this.enemyAllyMap[value] || `Unknown (${value})`;
        }
    }
});

customElements.define('game-info', GameInfo);
using Newtonsoft.Json;
using RpgBooster.Classes;
namespace RpgBooster
{
    public partial class RpgBooster : Form
    {
        private List<Panel> panels = new List<Panel>();
        private List<Player> jogadoresNaBatalha;
        private List<Player> jogadoresMortos = new List<Player>();
        private List<string> urlDosJogadores = new List<string>();

        private string Inventario;

        private decimal posiçãoDojogo;
        private decimal agilidadeDosPlayers;
        private decimal playerASerAtacado;

        private bool playerMorto = true;

        private Random random = new Random();

        public RpgBooster()
        {
            InitializeComponent();

            panels.Add(CadastrarPlayer);
            panels.Add(MudarEstatisticas);
            panels.Add(MudarInventario);
            panels.Add(InimigoEstatos);
            panels.Add(EscolherPlayers);
            panels.Add(Batalha);
        }

        //Eventos
        private void CadastrarPlayer_Click(object sender, EventArgs e)
        {
            CadastrarPlayer.Visible = true;

            LimparEstatisticas();
        }
        private void Salvar_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = $"{TextNomeCadastrarPlayer.Text}.json";

            if (TextNomeCadastrarPlayer.Text != "" || TextClasseCadastrarPlayer.Text != "")
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Player novoPlayer = new Player(TextNomeCadastrarPlayer.Text, TextClasseCadastrarPlayer.Text, NumNivelCadastrarPlayer.Value, NumExpCadastrarPlayer.Value, NumForçaCadastrarPlayer.Value, NumPersistenciaCadastrarPlayer.Value, NumSorteCadastrarPlayer.Value, NumAgilidadeCadastrarPlayer.Value, NumCarismaCadastrarPlayer.Value, NumInteligenciaCadastrarPlayer.Value, TextInvCadatrarPlayer.Text);

                    string player = JsonConvert.SerializeObject(novoPlayer, Formatting.Indented);
                    string url = saveFileDialog1.FileName;
                    File.WriteAllText(url, player);

                    foreach (var item in panels) item.Visible = false;
                }
            }
            else
            {
                MessageBox.Show("Nome ou Classe não pode ficar em branco!", "Erro");
            }
        }

        private void MudarInventario_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                CadastrarPlayer.Visible = true;
                MudarEstatisticas.Visible = true;
                MudarInventario.Visible = true;

                Player playerAntigo = JsonConvert.DeserializeObject<Player>(File.ReadAllText(openFileDialog1.FileName));
                TextInMudarInventario.Text = playerAntigo.Inventario;
            }
        }
        private void SalvarMudarInventario_Click(object sender, EventArgs e)
        {
            Player playerAntigo = JsonConvert.DeserializeObject<Player>(File.ReadAllText(openFileDialog1.FileName));

            saveFileDialog1.FileName = $"{playerAntigo.Nome}.json";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                playerAntigo.Inventario = TextInMudarInventario.Text;

                string Novoplayer = JsonConvert.SerializeObject(playerAntigo, Formatting.Indented);
                string url = saveFileDialog1.FileName;
                File.WriteAllText(url, Novoplayer);

                foreach (var item in panels) item.Visible = false;
            }
        }

        private void MudarEstatisticas_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (var item in panels) item.Visible = false;
                CadastrarPlayer.Visible = true;
                MudarEstatisticas.Visible = true;

                RestaurarEstatisticas();
            }
        }
        private void SalvarMudarEstatisticas_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = $"{TextNomeMudarEstatisticas.Text}.json";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Player novoPlayer = new Player(TextNomeMudarEstatisticas.Text, TextClasseMudarEstatisticas.Text, NumNivelMudarEstatisticas.Value, NumExpMudarEstatisticas.Value, NumForcaMudarEstatisticas.Value, NumPersistenciaMudarEstatisticas.Value, NumSorteMudarEstatisticas.Value, NumAgilidadeMudarEstatisticas.Value, NumCarismaMudarEstatisticas.Value, NumInteligenciaMudarEstatisticas.Value, Inventario);

                string player = JsonConvert.SerializeObject(novoPlayer, Formatting.Indented);
                string url = saveFileDialog1.FileName;
                File.WriteAllText(url, player);

                foreach (var item in panels) item.Visible = false;
            }
        }

        private void ComecarBatalha_Click(object sender, EventArgs e)
        {
            CadastrarPlayer.Visible = true;
            MudarEstatisticas.Visible = true;
            MudarInventario.Visible = true;
            InimigoEstatos.Visible = true;

            LimparEstatisticas();
        }
        private void ContinuarInimigoEstatos_Click(object sender, EventArgs e)
        {
            EscolherPlayers.Visible = true;
        }
        private void Adicionar_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Player player = JsonConvert.DeserializeObject<Player>(File.ReadAllText(openFileDialog1.FileName));

                jogadoresNaBatalha.Add(player);
                urlDosJogadores.Add(openFileDialog1.FileName);

                PlayerEscolhidos.Text += $" {player.Nome},";
            }
        }
        private void ContinuarEscolherPlayer_Click(object sender, EventArgs e)
        {
            if (jogadoresNaBatalha.Count >= 1)
            {
                Batalha.Visible = true;

                posiçãoDojogo = 0;

                foreach (var item in jogadoresNaBatalha)
                {
                    agilidadeDosPlayers += item.Agilidade;
                }

                if (NumAgilidadeInimigoEstatos.Value >= agilidadeDosPlayers)
                {
                    posiçãoDojogo += jogadoresNaBatalha.Count;
                }

                AtualizarEstatisticasBatalha(jogadoresNaBatalha[0]);
                LabInimigoNome.Text = TextNomeInimigoEstatos.Text;
                NumInimigoVida.Value = NumVidaInimigoEstatos.Value;
                NumInimigoDano.Value = NumDanoInimigoEstatos.Value;
                NumInimigoDanoCritico.Value = NumDanoCInimigoEstatos.Value;
            }
            else MessageBox.Show("Adicione um player a batalha", "Erro");
        }
        private void ProximoRound_Click(object sender, EventArgs e)
        {
            if (posiçãoDojogo >= jogadoresNaBatalha.Count)
            {
                VerificarSePerderam(jogadoresNaBatalha[0]);
                AtualizarEstatisticasBatalha(jogadoresNaBatalha[0]);

                while (playerMorto)
                {
                    playerASerAtacado = Math.Floor((decimal)random.NextDouble() * jogadoresNaBatalha.Count);

                    if (jogadoresNaBatalha[(int)playerASerAtacado].Vida > 1)
                    {
                        playerMorto = false;
                    }
                }

                TextHistoricoDaBatalha.Text += $"O {LabInimigoNome} atacou o {jogadoresNaBatalha[(int)playerASerAtacado].Nome}";

                if (NumInimigoDanoDado.Value >= 15)
                {
                    TextHistoricoDaBatalha.Text += $"com dano critico; ";
                    jogadoresNaBatalha[(int)playerASerAtacado].Vida -= NumInimigoDanoCritico.Value;
                }
                else jogadoresNaBatalha[(int)playerASerAtacado].Vida -= NumInimigoDano.Value;
            }
            else
            {
                AtualizarEstatisticasBatalha(jogadoresNaBatalha[(int)posiçãoDojogo]);

                TextHistoricoDaBatalha.Text += $"O {LabPlayerNome.Text} atacou o {LabInimigoNome.Text}; ";

                NumInimigoVida.Value -= NumPlayerDanoDado.Value;
                NumInimigoVida.Value -= NumPlayerForça.Value;
                NumInimigoVida.Value -= NumPlayerDanoItem.Value;
                NumInimigoVida.Value -= NumPlayerDanoEquipamento.Value;
            }

            posiçãoDojogo++;
            if (posiçãoDojogo >= (jogadoresNaBatalha.Count + NumQuantidadeInimigoEstatos.Value))
            {
                posiçãoDojogo = 0;
            }

            VerificarSePerderam(jogadoresNaBatalha[0]);
            VerificarSeGanharam();
        }
        private void ContinuarGanharam_Click(object sender, EventArgs e)
        {
            int index = 0;
            foreach (var item in jogadoresNaBatalha)
            {
                item.Exp += NumExpGanho.Value;

                while (item.Exp >= NumExpParaEvoluirLevel.Value)
                {
                    item.Nivel++;
                    item.Exp -= NumExpParaEvoluirLevel.Value;

                    MessageBox.Show($"{item.Nome} evoluiu de level");
                }

                string player = JsonConvert.SerializeObject(item, Formatting.Indented);
                string url = urlDosJogadores[index];
                File.WriteAllText(url, player);

                index++;
            }

        }

        //Click Voltar
        private void Voltar_Click(object sender, EventArgs e)
        {
            foreach (var item in panels) item.Visible = false;
        }

        //Metodos
        private void LimparEstatisticas()
        {
            TextNomeCadastrarPlayer.Text = "";
            TextClasseCadastrarPlayer.Text = "";
            NumNivelCadastrarPlayer.Value = 0;
            NumExpCadastrarPlayer.Value = 0;
            NumForçaCadastrarPlayer.Value = 0;
            NumPersistenciaCadastrarPlayer.Value = 0;
            NumSorteCadastrarPlayer.Value = 0;
            NumAgilidadeCadastrarPlayer.Value = 0;
            NumCarismaCadastrarPlayer.Value = 0;
            NumInteligenciaCadastrarPlayer.Value = 0;

            TextNomeInimigoEstatos.Text = "";
            NumVidaInimigoEstatos.Value = 0;
            NumDanoInimigoEstatos.Value = 0;
            NumDanoCInimigoEstatos.Value = 0;
            NumAgilidadeInimigoEstatos.Value = 0;
            NumQuantidadeInimigoEstatos.Value = 0;

            jogadoresNaBatalha = new List<Player>();
        }
        private void RestaurarEstatisticas()
        {
            Player player = JsonConvert.DeserializeObject<Player>(File.ReadAllText(openFileDialog1.FileName));

            TextNomeMudarEstatisticas.Text = player.Nome;
            TextClasseMudarEstatisticas.Text = player.Classe;
            NumNivelMudarEstatisticas.Value = player.Nivel;
            NumExpMudarEstatisticas.Value = player.Exp;
            NumForcaMudarEstatisticas.Value = player.Força;
            NumPersistenciaMudarEstatisticas.Value = player.Persistência;
            NumSorteMudarEstatisticas.Value = player.Sorte;
            NumAgilidadeMudarEstatisticas.Value = player.Agilidade;
            NumCarismaMudarEstatisticas.Value = player.Carisma;
            NumInteligenciaMudarEstatisticas.Value = player.Inteligência;
            Inventario = player.Inventario;
        }
        private void AtualizarEstatisticasBatalha(Player player)
        {
            decimal dado = Math.Floor((decimal)random.NextDouble() * 20);
            NumPlayerDanoDado.Value = dado;
            NumInimigoDanoDado.Value = dado;

            LabPlayerNome.Text = player.Nome;
            NumPlayerVida.Value = player.Vida;
            NumPlayerForça.Value = player.Força;
        }
        private void VerificarSePerderam(Player player)
        {
            if (player.Vida >= 0)
            {
                jogadoresMortos.Add(player);
            }

            if (jogadoresNaBatalha.Count == jogadoresMortos.Count)
            {
                GameOver.Visible = true;
            }
        }
        private void VerificarSeGanharam()
        {
            if (NumInimigoVida.Value < 1)
            {
                GameOver.Visible = true;
                Ganharam.Visible = true;
            }
        }
    }
}

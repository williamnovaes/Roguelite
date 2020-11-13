using UnityEngine;

public class Teste : MonoBehaviour {
    int width = 5, height = 5;
    int[] posicoes;

    private void Start() {
        posicoes = new int[width * height];

        for (int column = 0; column < width; column++) {
            for (int row = 0; row < height; row++) {
                int index = column + (row * width);
                SetTipo(index);
            }
        }
    }
    
    void SetTipo(int index) {
        Tipo tipo = GetTipo(index % width);
        string log = "";
        switch (tipo) {
            case Tipo.Esquerda:
                //validar se é corner cima, baixo ou nao
                if (index == 0) {
                    log = "Canto Superior Esquerdo";
                } else if ((width * height) - width == index) {
                    log = "Canto Inferior Esquerdo";
                } else {
                    log = "Canto Centro Esquerdo";
                }
                break;
            case Tipo.Centro:
                //validar se é borda cima, borda baixo ou centro
                if (width - index > 0) {
                    log = "Centro Superior";
                } else if (index % width == 0) {
                    log = "Centro Esquerdo";
                } else if (index % width == width - 1) {
                    log = "Centro Direito";
                } else if (index > width * height - width) {
                    log = "Centro Baixo";
                } else {
                    log = "Centro";
                }
                break;
            case Tipo.Direita:
                //Validar se é corner cima, baixo ou nao
                if (width - 1 == index) {
                    log = "Canto Superior Direito";
                } else if (width * height - 1 == index) {
                    log = "Canto Inferior Direito";
                } else {
                    log = "Canto Centro Direito";
                }
                break;
            default:
                log = "Error! Combinacao nao encontrada";
                break;
        }
        Debug.Log(log + ", index: " + index);
        // if (tipo == 0) {
        //     Debug.Log("Index Lado Direito "+ index);
        // }

        // if (tipo == width - 1 && ) {
        //     Debug.Log("Index Lado Esquerdo " + index);
        // }

        // if (tipo == 0 && index == 0) {
        //     Debug.Log("Corner Esquerdo Cima " + index);
        // }

        // if (index == width - 1) {
        //     Debug.Log("Corner Direito Cima " + index);
        // }

        // if (width * height - width == index) 
        // {
        //     Debug.Log("Corner Esquerdo Baixo " + index);
        // }

        // if (width * height - 1 == index)
        // {
        //     Debug.Log("Corner Direito Baixo " + index);
        // }
    }

    public Tipo GetTipo(int mod) {
        Tipo tipo;
        if (mod == 0) {
            tipo = Tipo.Esquerda;
        } else if (mod == width - 1) {
            tipo = Tipo.Direita;
        } else {
            tipo = Tipo.Centro;
        }
        Debug.Log(tipo);
        return tipo;
    }

    public enum Tipo {
        Esquerda = 0,
        Centro = 1,
        Direita = 2
    }
}
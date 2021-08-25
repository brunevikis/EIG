using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigMedicoes.Win.views {

    public interface ITreeNodeEditor {

        string SearchKey { get; set; }
        string Text { get; set; }

        void ShowEditor();

    }

    public interface ITreeNodeEditor<T> : ITreeNodeEditor where T : Modelo.Modelo {
        T BoundObject { get; set; }
    }

    public class TreeNodeEditor<T, U> : TreeNode, ITreeNodeEditor<T>
        where T : Modelo.Modelo
        where U : EditorUserControl<T> {

        public viewCadastros parent;

        public T BoundObject {
            get;
            set;
        }

        public string SearchKey {
            get;
            set;
        }

        public void ShowEditor() {

            if (parent.Editor == null) {
                parent.Editor = (UserControl)Activator.CreateInstance(typeof(U), parent);
            } else if (!(parent.Editor.GetType() == typeof(U))) {
                parent.Editor = (UserControl)Activator.CreateInstance(typeof(U), parent);
            }

            ((EditorUserControl<T>)parent.Editor).Bind(this);
        }

        public TreeNodeEditor(T rede, viewCadastros parent) {

            if (rede != null) {

                BoundObject = rede;

                Name = "node_" + rede.Nome.Replace(' ', '_');
                Tag = rede.Nome;
                SearchKey = Text = rede.Nome;

            }

            this.parent = parent;
        }
    }

    public interface IEditorUserControl {
        event EventHandler ActionDone;
        bool ViewOnly { get; set; }

    }

    public abstract partial class EditorUserControl<T> : UserControl, IEditorUserControl where T : Modelo.Modelo {

        public abstract T Model { get; set; }
        public ITreeNodeEditor<T> Node;


        public EditorUserControl() { }
        public EditorUserControl(viewCadastros parent) { }

        public virtual bool ViewOnly { get; set; }

        public virtual void Bind(ITreeNodeEditor<T> node) {
            this.Node = node;
            //this.Bind(node.BoundObject);
            this.Model = node.BoundObject;
        }
        public virtual void Bind(T model) {
            this.Model = model;
        }


        public virtual void UpdateModel() {
            Node.Text = Node.SearchKey = Model.Nome;
        }

        protected void InvokeActionDone() {
            if (ActionDone != null) {
                ActionDone(this, null);
            }
        }

        public event EventHandler ActionDone;

        private void InitializeComponent() {
            this.SuspendLayout();
            // 
            // EditorUserControl
            // 
            this.Name = "EditorUserControl";

            this.ResumeLayout(false);

        }
    }
}

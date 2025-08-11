using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FurkanKambay.Editor
{
    [CustomEditor(typeof(DeckHolder))]
    public class DeckHolderEditor : UnityEditor.Editor
    {
        private DeckHolder deckHolder;

        public override VisualElement CreateInspectorGUI()
        {
            deckHolder = target as DeckHolder;

            var root = new VisualElement();

            var defaultInspector = new VisualElement();
            root.Add(defaultInspector);

            InspectorElement.FillDefaultInspector(defaultInspector, serializedObject, this);

            if (!deckHolder)
                return root;

            var buttons = new VisualElement { enabledSelf = Application.isPlaying };
            root.Add(buttons);

            buttons.Add(new Button(DrawHand_Clicked) { text    = "Draw Hand" });
            buttons.Add(new Button(DiscardHand_Clicked) { text = "Discard Hand" });
            buttons.Add(new Button(ResetDeck_Clicked) { text   = "Reset Deck" });

            var state = new VisualElement { enabledSelf = Application.isPlaying };

            root.Add(state);

            if (!Application.isPlaying)
                return root;

            // Pile Lists
            var drawPileList = new ListView(deckHolder.Deck.DrawPile.ListRO)
            {
                showFoldoutHeader          = true,
                headerTitle                = "Draw Pile",
                bindingSourceSelectionMode = BindingSourceSelectionMode.AutoAssign
            };

            var handList = new ListView(deckHolder.Deck.HandPile.ListRO)
            {
                showFoldoutHeader          = true,
                headerTitle                = "Hand",
                bindingSourceSelectionMode = BindingSourceSelectionMode.AutoAssign
            };

            var discardPileList = new ListView(deckHolder.Deck.DiscardPile.ListRO)
            {
                showFoldoutHeader          = true,
                headerTitle                = "Discard Pile",
                bindingSourceSelectionMode = BindingSourceSelectionMode.AutoAssign
            };

            state.Add(drawPileList);
            state.Add(handList);
            state.Add(discardPileList);

            return root;
        }

        private void ResetDeck_Clicked()
        {
            if (deckHolder)
                deckHolder.ResetToStarterDeck();
        }

        private void DrawHand_Clicked()
        {
            if (deckHolder)
                deckHolder.DrawHand();
        }

        private void DiscardHand_Clicked()
        {
            if (deckHolder)
                deckHolder.DiscardHand();
        }
    }
}

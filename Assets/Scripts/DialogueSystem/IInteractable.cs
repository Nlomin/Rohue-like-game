using UnityEngine;

public interface IInteractable
{
    Transform GetTransform();          // Для расстояния
    void Interact();                   // Что происходит при взаимодействии
    void ShowInteractIcon(string keyText);     // Показать подсказку
    void HideInteractIcon();                   // Скрыть подсказку


}


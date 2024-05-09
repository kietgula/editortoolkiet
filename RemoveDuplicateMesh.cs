using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RemoveDuplicateMesh {
    [MenuItem("GameObject/Destroy duplicate children", priority = 50006)]
    private static void DestroyDuplicateChildren() {
        List<GameObject> parentGameObjects = GetSelectedGameObjects();
        if (!parentGameObjects.Any()) {
            return;
        }

        List<GameObject> allDirectChildren = new List<GameObject>();
        foreach (GameObject parent in parentGameObjects) {
            foreach (Transform childTrs in parent.transform) {
                allDirectChildren.Add(childTrs.gameObject);
            }
        }

        Dictionary<string, LinkedList<GameObject>> namePairGameObjects = new Dictionary<string, LinkedList<GameObject>>();
        foreach (GameObject directChild in allDirectChildren) {
            if (!namePairGameObjects.ContainsKey(directChild.name)) {
                namePairGameObjects.Add(directChild.name, new LinkedList<GameObject>());
            }
            LinkedList<GameObject> namedList = namePairGameObjects[directChild.name];
            namedList.AddLast(directChild);
        }

        int count = 0;
        foreach (KeyValuePair<string, LinkedList<GameObject>> entry in namePairGameObjects) {
            LinkedList<GameObject> namedList = entry.Value;
            LinkedListNode<GameObject> toKeep = namedList.First;
            bool foundDuplicate = true;
            while (foundDuplicate) {
                foundDuplicate = false;
                for (LinkedListNode<GameObject> node = toKeep.Next; node != null;) {
                    if (AreTheTwoGameObjectsTheSame(toKeep.Value, node.Value)) {
                        foundDuplicate = true;
                        Debug.Log($"Destroying {node.Value.name} from {node.Value.transform.parent.parent.name}");
                        Undo.DestroyObjectImmediate(node.Value);
                        count++;
                        LinkedListNode<GameObject> next = node.Next;
                        namedList.Remove(node);
                        node = next;
                        continue;
                    }
                    node = node.Next;
                }
                if (toKeep.Next == null) {
                    break;
                }
                toKeep = toKeep.Next;
                foundDuplicate = true;
            }
        }
        Debug.Log($"Total kills: {count}");
    }

    private static bool AreTheTwoGameObjectsTheSame(GameObject first, GameObject second) {
        if (!SameAll(first, second)) {
            return false;
        }

        List<GameObject> firstChildren = new List<GameObject>();
        foreach (Transform firstChild in first.transform) {
            firstChildren.Add(firstChild.gameObject);
        }
        for (int i = 0; i < firstChildren.Count; i++) {
            Transform temp = second.transform.GetChild(i);
            if (temp == null) {
                return false;
            }
            GameObject correspondingSecondChild = temp.gameObject;
            if (!AreTheTwoGameObjectsTheSame(firstChildren[i], correspondingSecondChild)) {
                return false;
            }
        }
        return true;
    }

    private static bool SameAll(GameObject first, GameObject second) {
        return SameName(first, second) &&
            SamePosition(first, second) &&
            SameRotation(first, second) &&
            SameChildrenCount(first, second);
    }

    private static bool SameName(GameObject first, GameObject second) {
        return first.name == second.name;
    }

    private static bool SamePosition(GameObject first, GameObject second) {
        float maxPositionDiff = 0.1f;
        return (first.transform.position - second.transform.position).sqrMagnitude <= maxPositionDiff;
    }

    private static bool SameRotation(GameObject first, GameObject second) {
        float maxRotationDiff = 0.1f;
        return (first.transform.rotation.eulerAngles - second.transform.rotation.eulerAngles).sqrMagnitude <= maxRotationDiff;
    }

    private static bool SameChildrenCount(GameObject first, GameObject second) {
        return first.transform.childCount == second.transform.childCount;
    }

    private static List<GameObject> GetSelectedGameObjects() {
        List<GameObject> tempList = new List<GameObject>();
        foreach (UnityEngine.Object @object in Selection.objects) {
            if (@object is GameObject gameObject) {
                tempList.Add(gameObject);
            }
            else {
                UnityEngine.Debug.LogError("One or more selected objects aren't GameObject");
                return new List<GameObject>();
            }
        }
        return tempList;
    }
}

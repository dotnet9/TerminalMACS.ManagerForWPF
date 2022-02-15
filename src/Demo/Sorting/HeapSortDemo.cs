namespace Sorting;

/// <summary>
///     堆排序（Heap Sort）
/// </summary>
internal class HeapSortDemo
{
    //public static void Main(string[] args)
    //{
    //	int[] arr = { 50, 10, 90, 30, 70, 40, 80, 60, 20 };
    //	Console.WriteLine("排序之前：");
    //	for (int i = 0; i < arr.Length; i++)
    //	{
    //		Console.Write(arr[i] + " ");
    //	}

    //	// 堆排序
    //	HeapSort(arr);

    //	Console.WriteLine();
    //	Console.WriteLine("排序之后：");
    //	for (int i = 0; i < arr.Length; i++)
    //	{
    //		Console.Write(arr[i] + " ");
    //	}
    //}

    /// <summary>
    ///     堆排序
    /// </summary>
    /// <param name="arr"></param>
    private static void HeapSort(int[] arr)
    {
        // 将待排序的序列构建成一个大顶堆
        for (var i = arr.Length / 2; i >= 0; i--) HeapAdjust(arr, i, arr.Length);

        // 逐步将每个最大值的根节点与末尾元素交换，并且再调整二叉树，使其成为大顶堆
        for (var i = arr.Length - 1; i > 0; i--)
        {
            swap(arr, 0, i); // 将堆顶记录和当前未经排序子序列的最后一个记录交换
            HeapAdjust(arr, 0, i); // 交换之后，需要重新检查堆是否符合大顶堆，不符合则要调整
        }
    }

    /// <summary>
    ///     构建堆的过程
    /// </summary>
    /// <param name="arr">需要排序的数组</param>
    /// <param name="i">需要构建堆的根节点的序号</param>
    /// <param name="n">数组的长度</param>
    private static void HeapAdjust(int[] arr, int i, int n)
    {
        int child;
        int father;
        for (father = arr[i]; leftChild(i) < n; i = child)
        {
            child = leftChild(i);

            // 如果左子树小于右子树，则需要比较右子树和父节点
            if (child != n - 1 && arr[child] < arr[child + 1]) child++; // 序号增1，指向右子树

            // 如果父节点小于孩子结点，则需要交换
            if (father < arr[child])
                arr[i] = arr[child];
            else
                break; // 大顶堆结构未被破坏，不需要调整
        }

        arr[i] = father;
    }

    // 获取到左孩子结点
    private static int leftChild(int i)
    {
        return 2 * i + 1;
    }

    // 交换元素位置
    private static void swap(int[] arr, int index1, int index2)
    {
        var tmp = arr[index1];
        arr[index1] = arr[index2];
        arr[index2] = tmp;
    }
}
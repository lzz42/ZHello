#include <stdio.h>

/*
定义链表

*/

/// <summary>
/// 一个包含指向自己的指针的结构体
/// </summary>
struct node {
	char* item;
	struct node* next;
};

typedef struct node* NodePtr;




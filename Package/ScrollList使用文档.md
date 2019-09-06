#  Scroll List使用文档
## 使用方法
- 右键创建或者从Prefab中拖到场景中
- 在Inspector中配置Item预制件
- 编写ItemAdapter，继承于抽象类BaseItemAdapter，实现Init方法和Update方法，分别是初始化item与更新Item。
- 编写ListAdapter或者使用默认的ListAdapter，继承于抽象类BaseListAdapter，用于将数据集合解释给List，默认的ListAdapter仅支持List类型数据集合，其他类型集合需要手动编写。
- 使用SetAdapter方法给ScrollList设置适配器即可显示数据。
- 只用修改ListAdapter对应的List就可以更新界面，如果需要给Item绑定监听，建议在ItemAdapter的Init方法中绑定。

## Inspector界面
- Item Pre    item预制件，决定item样式
- Divider      分割线预制件，决定item之间的分割线样式，可以不设置。
- Divider Size 分割线占据的空间
- Type          ScrollList的类型，横向和纵向
- Per Page Count        每页显示多少个Item
- Pre Load Count        预加载的Item数量
- Show Scroll Bar       是否显示滚动条


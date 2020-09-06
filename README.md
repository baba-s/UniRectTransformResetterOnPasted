# UniRectTransformResetterOnPasted

UI オブジェクトをコピペした時に Pos Z と Scale に変な値が入らないようにするエディタ拡張  

## 使い方

![2020-09-06_115808](https://user-images.githubusercontent.com/6134875/92317250-c4932080-f039-11ea-8c89-cdddf3743765.png)

Canvas の「Render Mode」を「Screen Space - Camera」にして「Render Camera」を設定している場合  
Canvas の「Pos Z」と「Scale」の値が変化しますが、  

![image (33)](https://user-images.githubusercontent.com/6134875/92317252-c5c44d80-f039-11ea-9f1f-1b4401f798fe.gif)

この状態で UI オブジェクトを別の階層にコピペすると  
コピペした UI オブジェクトの「Pos Z」と「Scale」の値が Canvas の値で上書きされてしまい、  
画面に正常に表示されなくなってしまいます（「Pos Z」と「Scale」をデフォルト値に戻すと正常に表示される）  

![image (34)](https://user-images.githubusercontent.com/6134875/92317253-c5c44d80-f039-11ea-84c8-183cf3507442.gif)

UniRectTransformResetterOnPasted を Unity プロジェクトに導入した状態で  
Ctrl + V で UI オブジェクトを貼り付けると  
コピペした UI オブジェクトの「Pos Z」と「Scale」の値が Canvas の値で上書きされないようになります  

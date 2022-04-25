import{_ as t}from"./AlertModal-0eded1fe.js";import{a as e,b as a,_ as s}from"./EditNavbar-5265f9c5.js";import{_ as r}from"./FormInputSelect-54f9c2aa.js";import{ap as i,aq as o,ar as l,_ as n,v as c,r as m,o as d,c as h,e as u,d as p,w as f,t as y,E as g,aw as I,F as V,l as b}from"./vendor-358810ac.js";import{_ as x,m as w,e as A,n as v,o as E,i as H}from"./index-8468366b.js";import"./tippy.esm-6c7ac1c0.js";const $={name:"EditNormal",components:{EditNavbar:e,Switch:i,SwitchGroup:o,SwitchLabel:l,FormInputSelect:r,Input:a,TipTap:s,AlertModal:t},props:{articleId:{type:[String,Number],default:""}},data:()=>({category:[],articleVm:{userName:"",title:"",isFree:!0,isPush:!0,articlecategoryId:null,introduction:"",main:""},coverImage:null,categoryVm:null,artInfoCount:0,mainCount:0,isMainEmpty:!0,errors:[],editMode:!1,alertInfo:null}),computed:{categoryHandler:{get(){return this.categoryVm},set(t){this.categoryVm=t,this.articleVm.articlecategoryId=t.Id}}},async mounted(){this.articleId?(this.editMode=!0,await w(this.articleId).then((t=>{t.data.success?Object.assign(this.articleVm,t.data.data):this.$notify({group:"error",title:"文章資料取得失敗"})})).catch((t=>{}))):this.editMode=!1,await A().then((t=>{if(this.category=[...t.data],this.editMode){const t=this.articleVm.articlecategoryId,e=this.category.find((e=>e.Id===t));this.categoryHandler=e}})),this.articleVm.userName=this.$store.state.userInfo.Username},methods:{postArticle(){this.$store.commit("SHOW_OVERLAY_LOADING"),v(this.articleVm).then((t=>{if(t.data.success){const e={message:this.articleVm.isPush?"文章已發布 !":"文章已儲存",confirmText:this.articleVm.isPush?"去看內文":"回個人頁面",confirmMode:"replace",confirmTodo:{name:this.articleVm.isPush?"ArticleCommon":"UserDetail",params:this.articleVm.isPush?{articleId:t.data.artId}:{userId:this.$store.state.userInfo.Username,target:"articles"}}};this.alertInfo=e,this.$store.commit("HIDE_OVERLAY_LOADING"),this.$store.commit("SHOW_ALERT")}else this.$store.commit("HIDE_OVERLAY_LOADING"),this.$notify({group:"error",title:"發布失敗",message:t.data.message})})).catch((t=>{this.$store.commit("HIDE_OVERLAY_LOADING")}))},editArticle(){this.$store.commit("SHOW_OVERLAY_LOADING"),E(this.articleId,this.articleVm).then((t=>{if(t.data.success){let t={};t=this.articleVm.isPush?{message:"文章已發布 !",confirmText:"去看內文",confirmMode:"replace",confirmTodo:{name:"ArticleCommon",params:{articleId:this.articleId}}}:{message:"文章已儲存 !",confirmText:"回到個人主頁",confirmMode:"replace",confirmTodo:{name:"UserDetail",params:{userId:this.$store.state.userInfo.Username,target:"articles"}}},this.alertInfo=t,this.$store.commit("SHOW_ALERT")}else this.$notify({group:"error",title:"發布失敗",message:t.data.message});this.$store.commit("HIDE_OVERLAY_LOADING")})).catch((t=>{}))},saveHandler(){this.alertInfo={},this.articleVm.isPush=!1;const t=this.checkHandler(this.articleVm);t.success?this.editMode?this.editArticle():this.postArticle():(this.alertInfo={message:`${t.errors[0].message}`,showConfirm:!1,showCancel:!0,cancelText:"返回",cancelMode:"anchor",cancelTodo:{name:this.$route.name,params:{article:this.articleId},hash:`#${t.errors[0].anchor}`}},this.$store.commit("SHOW_ALERT"))},publishHandler(){this.alertInfo={},this.articleVm.isPush=!0;const t=this.checkHandler(this.articleVm);t.success?this.editMode?this.editArticle():this.postArticle():(this.alertInfo={message:`${t.errors[0].message}`,showConfirm:!1,showCancel:!0,cancelText:"返回",cancelMode:"anchor",cancelTodo:{name:this.$route.name,params:{article:this.articleId},hash:`#${t.errors[0].anchor}`}},this.$store.commit("SHOW_ALERT"))},cancelEdit(){this.alertInfo={message:"確定返回上一頁 ?",confirmMode:"push",confirmTodo:{path:this.$store.state.recordPath}},this.$store.commit("SHOW_ALERT")},sendCover(t){const e=new FormData;e.append("photo",this.coverImage.file),H(e).then((e=>{e.data.success?this.articleVm.firstPhoto=e.data.picname:(this.$notify({group:"error",title:"上傳圖片失敗 !"}),this.articleVm.firstPhoto=t)})).catch((t=>{}))},titleHandler(t){this.articleVm.title=this.titleError(t)},titleError(t){const e=this.errors.findIndex((e=>e===t.name));return t.error?(-1===e&&this.errors.push(t.name),""):(-1!==e&&this.errors.splice(e,1),t.value)},artInfoCountHandler:n.throttle((function(t){this.artInfoCount=t}),500),mainCountHandler:n.throttle((function(t){this.mainCount=t}),500),addTool(){this.articleVm.fArrayList.push({uuid:c(),secPhoto:"",mission:""})},checkHandler(t){const e=[];return t.title||e.push({key:"title",anchor:"editor-normal-title",message:"文章標題為必填 !"}),t.articlecategoryId||e.push({key:"articlecategoryId",anchor:"editor-normal-category",message:"全站分類為必填 !"}),this.isMainEmpty&&e.push({key:"main",anchor:"editor-normal-main",message:"文章內容不可為空 !"}),e.length?{success:!1,errors:e,message:"必填欄位未填"}:{success:!0,message:"文章編輯檢查成功"}},initData(t){this.$data[t]=this.$options.data()[t]}}},_={class:"py-11 px-4 mx-auto max-w-[816px] md:px-8"},C={class:"kiruPartEffect"},L={id:"editor-normal-title",class:"mb-12"},O=p("div",{class:"mb-2"},[p("h2",{class:"inline-block pr-4 text-xl font-bold text-myBrown border-r-2 border-myBrown md:mb-2 md:text-2xl"}," 文章標題 ")],-1),S=p("div",{class:"mb-4 md:mb-6"},[p("h2",{class:"inline-block pr-4 mb-1 text-lg font-bold text-myBrown border-r-2 border-myBrown md:mb-2 md:text-2xl"}," 編輯資訊 ")],-1),T={class:"kiruPartEffect"},M={id:"editor-normal-pay",class:"mb-6 md:mb-8"},P={class:"flex gap-8 items-center"},B=p("span",{class:"font-bold text-myBrown"}," 是否要把這篇設為付費閱讀 ? ",-1),k=p("span",{class:"text-myBrown material-icons"},"info",-1),D=p("span",{class:"sr-only"},"是否要把這篇設為付費閱讀",-1),N={id:"editor-normal-category",class:"mb-8 w-full sm:w-1/3"},R=p("h3",{class:"font-bold text-myBrown"}," 全站分類 ",-1),U={id:"editor-normal-introduction",class:"py-4 mb-6"},W={class:"flex items-end mb-2"},j=p("h3",{class:"font-bold text-myBrown"}," 文章敘述 ",-1),F=p("span",{class:"py-0.5 px-2 text-sm text-myBrown"},"( 上限 150 字元 )",-1),G={class:"py-0.5 text-xs text-myBrown rounded"},Y={id:"editor-normal-main",class:"py-4 mb-12"},q={class:"mb-2"},z=p("h2",{class:"inline-block pr-4 text-2xl font-bold text-myBrown md:mb-2 md:text-3xl"}," 開始寫作 ",-1),J={class:"text-sm text-black/60"},K={class:"flex justify-center md:justify-start"};var Q=x($,[["render",function(i,o,l,n,c,x){const w=e,A=a,v=m("SwitchLabel"),E=m("Switch"),H=m("SwitchGroup"),$=r,Q=s,X=t;return d(),h(V,null,[u(w,{onSaveArticle:x.saveHandler,onPublishArticle:x.publishHandler},null,8,["onSaveArticle","onPublishArticle"]),p("div",_,[p("div",C,[p("div",L,[O,u(A,{class:"w-full sm:w-2/3","error-class":"md:-right-2 md:translate-x-full md:top-1/2 md:-translate-y-1/2 md:absolute",placeholder:"按這裡輸入標題",name:"title","error-text":"標題為必填","default-value":c.articleVm.title,onUpdate:x.titleHandler},null,8,["default-value","onUpdate"])])]),S,p("div",T,[p("div",M,[u(H,null,{default:f((()=>[p("div",P,[u(v,{class:"flex gap-2 items-center"},{default:f((()=>[B,k])),_:1}),u(E,{modelValue:c.articleVm.isFree,"onUpdate:modelValue":o[0]||(o[0]=t=>c.articleVm.isFree=t),class:b([c.articleVm.isFree?"bg-white":"bg-myBrown","mySwitchBar"])},{default:f((()=>[D,p("span",{class:b([c.articleVm.isFree?"translate-x-0":"translate-x-8","ring-myBrown mySwitchButton"])},null,2)])),_:1},8,["modelValue","class"])])])),_:1})]),p("div",N,[R,p("div",null,[u($,{modelValue:x.categoryHandler,"onUpdate:modelValue":o[1]||(o[1]=t=>x.categoryHandler=t),options:c.category,"key-prop":"Name","label-prop":"Name","default-text":"打開選單","options-position":"absolute"},null,8,["modelValue","options"])])]),p("div",U,[p("div",W,[j,F,p("span",G,"字數 : "+y(c.artInfoCount),1)]),u(Q,{modelValue:c.articleVm.introduction,"onUpdate:modelValue":o[2]||(o[2]=t=>c.articleVm.introduction=t),placeholder:"簡述一下這篇文章的內容吧 _","custom-class":"min-h-[6rem]","word-limit":"150",onWordCount:x.artInfoCountHandler},null,8,["modelValue","onWordCount"])])]),p("div",Y,[p("div",q,[z,p("span",J,"字數 : "+y(c.mainCount),1)]),u(Q,{modelValue:c.articleVm.main,"onUpdate:modelValue":o[3]||(o[3]=t=>c.articleVm.main=t),placeholder:"開始寫作吧 _","custom-class":"min-h-[18rem] md:min-h-[24rem] border-none py-2 px-0","allow-image":!0,onWordCount:x.mainCountHandler,onCheckEmpty:o[4]||(o[4]=t=>c.isMainEmpty=t)},null,8,["modelValue","onWordCount"])]),p("div",K,[p("button",{type:"button",class:"userPageCancelButton",onClick:o[5]||(o[5]=(...t)=>x.cancelEdit&&x.cancelEdit(...t))}," 取消編輯 ")])]),u(X,g(I(c.alertInfo)),null,16)],64)}]]);export{Q as default};

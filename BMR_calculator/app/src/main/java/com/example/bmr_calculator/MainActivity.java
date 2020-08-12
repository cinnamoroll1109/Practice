package com.example.bmr_calculator;

import androidx.appcompat.app.AppCompatActivity;
import android.content.Intent;
import android.net.http.AndroidHttpClient;
import android.os.Bundle;
import android.os.StrictMode;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpCookie;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;

public class MainActivity extends AppCompatActivity {
    private Button btn_create;
    private ListView listView;

    //String urladdress = "http://192.168.66.18/android_use/android_bmr_login.php";
    String urladdress = "http://10.0.60.21/android_use/android_bmr_login.php";

    String _id, _name, _sex, _age, _height, _weight, _bmr;

    String result = null;
    String database_return = null;

    String[] listview_test = new String[]{
            "apple","banana","grape","pear"
    };

    JSONArray jArray;

    ArrayList<String> al_id = new ArrayList<String>();
    ArrayList<String> al = new ArrayList<String>();
    ArrayList<String> al1 = new ArrayList<String>();
    ArrayList<String> al2 = new ArrayList<String>();
    ArrayList<String> al3 = new ArrayList<String>();
    ArrayList<String> al4 = new ArrayList<String>();
    ArrayList<String> al5 = new ArrayList<String>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        btn_create = findViewById(R.id.p1_btnCreate);
        listView = findViewById(R.id.p1_listview);

        Thread thread = new Thread(multithread);
        thread.start();

        ListAdapter adapter = new ArrayAdapter<>(this, android.R.layout.simple_list_item_1, al);
        listView.setAdapter(adapter);
        //click item of listview event
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {

                Bundle bundle = new Bundle();
                bundle.putString("id", al_id.get(i));
                bundle.putString("name", al.get(i));
                bundle.putString("sex", al1.get(i));
                bundle.putString("age", al2.get(i));
                bundle.putString("height", al3.get(i));
                bundle.putString("weight", al4.get(i));
                bundle.putString("bmr", al5.get(i));

                //切換頁面
                Intent it = new Intent();
                it.putExtras(bundle);
                it.setClass(MainActivity.this, Page4_modify.class);
                startActivity(it);
            }
        });

        //click button event
        View.OnClickListener btn_createOnClick=new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //切換頁面
                Intent it = new Intent();
                it.setClass(MainActivity.this, Page2_calculator.class);
                startActivity(it);
            }
        };
        btn_create.setOnClickListener(btn_createOnClick);

    }

    private void collectUsers(){
        Thread t = new Thread(new Runnable(){
            @Override
            public void run() {
                try {
                    URL url = new URL(urladdress);
                    HttpURLConnection conn = (HttpURLConnection)url.openConnection();
                    conn.setRequestMethod("POST");
                    conn.setDoOutput(true);
                    conn.setDoInput(true);
                    conn.setUseCaches(false);
                    conn.connect();     //start connection
                    database_return = "connect";

                    int responseCode = conn.getResponseCode();
                    if(responseCode == HttpURLConnection.HTTP_OK){
                        InputStream inputStream = conn.getInputStream();
                        BufferedReader bufReader =
                                new BufferedReader(new InputStreamReader(inputStream, "utf-8"), 8);
                        String box = "", line = null;
                        while((line = bufReader.readLine()) != null){
                            box += line + "\n";
                        }
                        inputStream.close();
                        database_return = box;
                    }

                }catch (Exception e){
                    database_return = e.toString();
                }
            }
        });
        t.start();
    }

    private Runnable multithread = new Runnable() {
        @Override
        public void run() {
            try{
                //URL url = new URL("http://192.168.66.18/android_use/android_bmr_login.php");
                URL url = new URL("http://10.0.60.21/android_use/android_bmr_login.php");

                //宣告HTTP連線需要的物件
                HttpURLConnection connection = (HttpURLConnection)url.openConnection();
                connection.setRequestMethod("POST"); //設定連線方式為POST
                connection.setDoOutput(true); //允許輸出
                connection.setDoInput(true); //允許讀入
                connection.setUseCaches(false); //不使用快取
                connection.connect(); //開始連線

                int responseCode = connection.getResponseCode(); //建立取得回應的物件
                if(responseCode == HttpURLConnection.HTTP_OK){
                    Log.e("DB:", "connect");
                    InputStream inputStream = connection.getInputStream(); //取得輸入串流
                    BufferedReader bufReader = new BufferedReader(new InputStreamReader(inputStream,
                            "utf-8"), 8); //取得輸入串流的資料
                    String box = ""; //宣告存放用字串
                    String line = null; //宣告讀取用的字串
                    while((line = bufReader.readLine()) != null){
                        box += line + "\n"; // 每當讀取出一列，就加到存放字串後面
                        Log.e("DB:", "Read lines");
                    }
                    inputStream.close();
                    result = box;
                }
                else{
                    result = "http not ok:" + responseCode;
                }
                // 讀取輸入串流並存到字串的部分
                // 取得資料後想用不同的格式
                // 例如 Json 等等，都是在這一段做處理
                try{
                    jArray = new JSONArray(result);
                    JSONObject json_data = null;
                    for (int i = 0; i < jArray.length(); i++) {
                        json_data = jArray.getJSONObject(i);
                        /*name = new String[json_data.length()];
                        sex = new String[json_data.length()];
                        age = new String[json_data.length()];
                        height = new String[json_data.length()];
                        weight = new String[json_data.length()];
                        bmr = new String[json_data.length()];

                        for(int j = 0; j<=json_data.length();j++){

                        }*/
                        _id = json_data.getString("id");
                        _name = json_data.getString("name");
                        _sex=json_data.getString("sex");
                        _age = json_data.getString("age");
                        _height = json_data.getString("height");
                        _weight=json_data.getString("weight");
                        _bmr = json_data.getString("bmr");

                        al_id.add(_id);
                        al.add(_name);
                        al1.add(_sex);
                        al2.add(_age);
                        al3.add(_height);
                        al4.add(_weight);
                        al5.add(_bmr);

                        //listItemCount=al2.size();
                    }
                }
                catch(JSONException e){
                    Toast.makeText(getApplicationContext(), e.toString(), Toast.LENGTH_LONG).show();
                }
            }catch (Exception e){
                result = e.toString();
            }

            runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    //test.setText(result);
                }
            });
        }
    };
}
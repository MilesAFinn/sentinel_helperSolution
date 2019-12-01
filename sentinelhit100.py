import json
import pandas as pd

#with open('sentinelresults-short.json', encoding="utf8") as f: # this ensures opening and closing file
with open('sentinelresults.json', encoding="utf8") as f: # this ensures opening and closing file
    a = json.loads(f.read())

data = a["documents"]

df = pd.DataFrame(data)

#print(df.transpose())
print(df)

export_csv = df.to_csv (r'export_dataframe4.csv', index = None, header=False, sep = '|')
